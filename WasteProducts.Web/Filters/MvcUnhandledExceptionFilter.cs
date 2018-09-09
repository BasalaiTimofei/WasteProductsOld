using System.Web.Mvc;
using Elmah.Contrib.Mvc;
using NLog;
using WasteProducts.Web.Utils.Logging;

namespace WasteProducts.Web.Filters
{
    /// <inheritdoc />
    public class MvcUnhandledExceptionFilter : ElmahHandleErrorAttribute
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">NLog logger</param>
        public MvcUnhandledExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
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