using Elmah.Contrib.Mvc;
using NLog;
using WasteProducts.Web.Utils;
using ExceptionContext = System.Web.Mvc.ExceptionContext;

namespace WasteProducts.Web.Filters
{
    public class MvcUnhandledExceptionFilter : ElmahHandleErrorAttribute
    {
        private readonly ILogger _logger;

        public MvcUnhandledExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            //NLog logging
            var routeData = context.RouteData;
            var controllerName = routeData.Values["controller"]?.ToString() ?? "Undefined controller";
            var actionName = routeData.Values["action"]?.ToString() ?? "Undefined action";

            _logger.ActionError(context.Exception, controllerName, actionName);

            //Elmah logging
            base.OnException(context);
        }
    }
}