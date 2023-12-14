using EcomTest.Domain.DomainEntities;
using EcomTest.Domain.DomainEntities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using System.Linq.Expressions;


namespace EcomTest.Common.DbContexts
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private IDbContextTransaction? _transaction;

        public EfRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdNoTrackingAsync(long id)
        {
             return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAsListAsync<TProperty>(string propertyName, IEnumerable<TProperty> values, bool trackEntities = true, Expression < Func<TEntity, bool>> additionalCondition = null)
        {
            // Create expression for property filter
            var property = typeof(TEntity).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on entity '{typeof(TEntity).Name}'.");
            }

            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var propertyExpression = Expression.Property(parameter, property);

            // Convert IEnumerable<TProperty> to constant array
            var valuesArray = values.ToArray();
            var valuesExpression = Expression.Constant(valuesArray, typeof(IEnumerable<TProperty>));

            // Create the Contains method call expression
            var containsMethod = typeof(Enumerable).GetMethods()
                .FirstOrDefault(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                ?.MakeGenericMethod(typeof(TProperty));

            if (containsMethod == null)
            {
                throw new InvalidOperationException("Unable to find the Contains method.");
            }

            var containsCallExpression = Expression.Call(containsMethod, valuesExpression, propertyExpression);

            // Combine the property filter with the additional condition (if provided)
            var body = additionalCondition != null
                ? Expression.AndAlso(Expression.Invoke(additionalCondition, parameter), containsCallExpression)
                : (Expression)containsCallExpression;

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            // Use the DbSet directly with the dynamic predicate - efficient as where clause on the db side, not bringing all in memory to then filter
            IQueryable<TEntity> query = trackEntities ? _dbSet : _dbSet.AsNoTracking();
            return await query.Where(lambda).ToListAsync();
        }


       
        public async Task<TEntity> GetSingleOrDefaultAsync<TProperty>(string propertyName, TProperty valueToFilterPropertyNameOn, bool enableTracking = true)
        {
            // Create expression for property filter
            var property = typeof(TEntity).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on entity '{typeof(TEntity).Name}'.");
            }

            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var propertyExpression = Expression.Property(parameter, property);

            // Create constant expression for the single value
            var valueExpression = Expression.Constant(valueToFilterPropertyNameOn, typeof(TProperty));

            // Create the Equals method call expression
            var equalsMethod = typeof(object).GetMethod("Equals", new[] { typeof(object) });
            var equalsCallExpression = Expression.Call(propertyExpression, equalsMethod, valueExpression);

            // Create lambda expression
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equalsCallExpression, parameter);

            // Use the DbSet directly with the dynamic predicate and apply AsNoTracking conditionally
            var query = enableTracking ? _dbSet : _dbSet.AsNoTracking();

            return await query.FirstOrDefaultAsync(lambda);
        }


        public async Task<TEntity> AddAndReturnCreatedAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var createdEntity = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return createdEntity.Entity;
        }

        public async Task UpdateMultipleAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }
    }
}
