using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace EcomTest.Web.Host.Startup.Extensions
{
    public static partial class SwaggerConfig
    {
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                // Resolve the temporary IApiVersionDescriptionProvider service
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                var productName = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;

                // Add a swagger document for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = description.IsDeprecated ? $"{productName} {description.ApiVersion} - DEPRECATED" : $"{productName}",
                        Version = description.ApiVersion.ToString()
                    });
                }

                //Configure auth if needed

                //Add action filter here if you want to be able to ommit endpoints / controllers from the Swagger documentation

            });
        }


        public static void UseCustomSwagger(this IApplicationBuilder app, IConfiguration configuration, IApiVersionDescriptionProvider provider)
        {
            var hostVirtualDirectory = configuration["App:HostVirtualDirectory"];
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint
            app.UseSwaggerUI(options =>
            {
                //Build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    if (!String.IsNullOrEmpty(hostVirtualDirectory))
                    {
                        options.SwaggerEndpoint($"/{hostVirtualDirectory}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                    else
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                }
                
                options.DocExpansion(DocExpansion.None); // Collapse tags on loading
                options.EnableFilter(); // Show/Enable tags filter
            });
        }

    }
}
