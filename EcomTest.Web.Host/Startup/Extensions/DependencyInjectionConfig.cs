using EcomTest.Web.Host.ActionFilters;
using System.Reflection;

namespace EcomTest.Web.Host.Startup.Extensions
{
    public static partial class DependencyInjectionConfig
    {
        public static void RegisterDIContainers(this IServiceCollection services)
        {
            //add controller auto logging for auditing purposes
            services.AddScoped<ControllerLogging>();

            // Use Reflection to auto setup newly added app services, all as transient (so new instance each time, stateless for the type of API we are creating)
            var appServiceRegistrations =
                from type in Assembly.Load("EcomTest.Application").GetTypes()
                where !type.IsInterface && !type.IsAbstract && !type.IsEnum && type.Name.ToUpper().Contains("APPSERVICE") && type.Namespace!= null && type.Namespace.Contains("ApplicationServices")
                select new { Interface = type.GetInterfaces().Single(), Implementation = type };
            foreach (var reg in appServiceRegistrations)
            {
                services.AddTransient(reg.Interface, reg.Implementation);
            }

            //as above for domain services
            var domainServiceRegistrations =
              from type in Assembly.Load("EcomTest.Domain").GetTypes()
              where !type.IsInterface && !type.IsAbstract && !type.IsEnum && type.Name.ToUpper().Contains("DOMAINSERVICE") && type.Namespace != null && type.Namespace.Contains("DomainServices")
              select new { Interface = type.GetInterfaces().Single(), Implementation = type };
            foreach (var reg in domainServiceRegistrations)
            {
                services.AddTransient(reg.Interface, reg.Implementation);
            }
        }
    }
}
