using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Common.DbContexts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdNoTrackingAsync(long id);

        /// <summary>
        /// Pass it a propery name and a value, and it will return the FirstOrDefault() record that matches the value passed in
        /// </summary>
        /// <typeparam name="TProperty">Entity type</typeparam>
        /// <param name="propertyName">e.g. nameof(Customer.Email)</param>
        /// <param name="valueToFilterPropertyNameOn"> e.g.emailaddress@email.com</param>
        /// <param name="enableTracking">Enables SQL / EF tracking on returned record or not, true by default</param>
        /// <returns></returns>
        Task<TEntity> GetSingleOrDefaultAsync<TProperty>(string propertyName, TProperty valueToFilterPropertyNameOn, bool enableTracking = true);

       
        /// <summary>
        ///  Pass it a propery name and a value, and it will return a list of records that match the value passed in
        /// </summary>
        /// <typeparam name="TProperty">Entity type</typeparam>
        /// <param name="propertyName">e.g.nameof(StockItem.Name)</param>
        /// <param name="valueToFilterPropertyNameOn">e.g. Stock Item Name 1</param>
        /// <param name="trackEntities">Enables SQL / EF tracking on returned records or not, true by default</param>
        /// <param name="additionalCondition">Condition to filter results on, SQL server side</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsListAsync<TProperty>(string propertyName, IEnumerable<TProperty> values, bool trackEntities = true, Expression<Func<TEntity, bool>> additionalCondition = null);

        Task UpdateMultipleAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Adds a new record of the entity type passed in, and returns the created record to obtain its Identity (Id)
        /// </summary>
        /// <param name="entity">Object of type /subtype Entity</param>
        /// <returns>Record of type passed in, with correct Identity Id after insert</returns>
        Task<TEntity> AddAndReturnCreatedAsync(TEntity entity);
      
    }
}
