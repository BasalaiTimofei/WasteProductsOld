using System.Web.Mvc;
using Elmah.Contrib.Mvc;
using NLog;
using WasteProducts.Web.Utils.Logging;

namespace WasteProducts.Web.Filters
{
    /// <inheritdoc />
    public class MvcUnhandledExceptionFilter : ElmahHandleErrorAttribute
    {
        private const string ControllerKey = "controller";
        private const string ActionKey = "action";
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
            if (context.RequestContext.RouteData.Values.ContainsKey(ControllerKey))
            {
                var controllerName = context.RouteData.Values[ControllerKey].ToString();
                var actionName = context.RouteData.Values[ActionKey].ToString();

                _logger.ActionError(context.Exception, controllerName, actionName);
            }

            //Elmah logging
            base.OnException(context);
        }
    }
}