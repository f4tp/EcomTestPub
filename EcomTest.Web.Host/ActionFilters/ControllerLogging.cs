using Microsoft.AspNetCore.Mvc.Filters;

namespace EcomTest.Web.Host.ActionFilters
{
    public class ControllerLogging : IActionFilter
    {
        private readonly ILogger<ControllerLogging> _logger;

        public ControllerLogging(ILogger<ControllerLogging> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _logger.LogTrace("Executing {0} with args:{2}", actionContext.ActionDescriptor.DisplayName, string.Join(";", actionContext.ActionArguments.Select(x => x.Key + "=" + x.Value).ToArray()));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogTrace("Executed action");
        }
    }
}
