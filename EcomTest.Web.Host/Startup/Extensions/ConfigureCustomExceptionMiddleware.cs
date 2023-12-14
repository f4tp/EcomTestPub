
using EcomTest.Web.Host.Middleware;

namespace EcomTest.Web.Host.Startup.Extensions
{
    public static partial class GlobalExceptionHandlingConfig
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
