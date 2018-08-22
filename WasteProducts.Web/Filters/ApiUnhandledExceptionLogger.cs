using System.Web.Http.ExceptionHandling;
using Elmah.Contrib.WebApi;
using NLog;
using WasteProducts.Web.Utils.Logging;

namespace WasteProducts.Web.Filters
{
    public class ApiUnhandledExceptionLogger : ElmahExceptionLogger
    {
        private readonly ILogger _logger;

        public ApiUnhandledExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            //NLog logging
            var requestContext = context.RequestContext;
            var controllerName = requestContext.RouteData.Values["controller"]?.ToString() ?? "Undefined controller";
            var actionName = context.Request.Method.Method;

            _logger.ActionError(context.Exception, controllerName, actionName);

            //Elmah logging
            base.Log(context);
        }
    }
}