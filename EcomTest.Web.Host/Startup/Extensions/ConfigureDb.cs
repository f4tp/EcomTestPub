using EcomTest.Common.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EcomTest.Web.Host.Startup.Extensions
{
    public static partial class ConfigureDb
    {
        public static void ConfigureDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(configuration["ConnectionStrings:EcommTestMain"], providerOptions => providerOptions.EnableRetryOnFailure() );
            });

            //inject so Repository pattern can be used
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }
    }
}
