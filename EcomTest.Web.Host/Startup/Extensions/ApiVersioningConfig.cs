

using Microsoft.AspNetCore.Mvc;

namespace EcomTest.Web.Host.Startup.Extensions
{
    public static partial class ApiVersioningConfig
    {
        /// <summary>
        /// Sets up versioning for the API endpoints, so newer versions can be added without affecting currently consumed instances
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}