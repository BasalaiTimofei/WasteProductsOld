using System.Linq;
using System.Web.Http.ExceptionHandling;
using Elmah.Contrib.WebApi;
using NLog;
using WasteProducts.Web.Utils.Logging;

namespace WasteProducts.Web.Filters
{
    /// <inheritdoc />
    public class ApiUnhandledExceptionLogger : ElmahExceptionLogger
    {
        private const string ControllerKey = "controller";
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">NLog logger</param>
        public ApiUnhandledExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override void Log(ExceptionLoggerContext context)
        {
            //NLog logging
            if (context.RequestContext.RouteData.Values.ContainsKey(ControllerKey))
            {
                var controllerName = context.RequestContext.RouteData.Values[ControllerKey].ToString();
                var actionName = context.Request.Method.Method;

                _logger.ActionError(context.Exception, controllerName, actionName);
            }

            //Elmah logging
            base.Log(context);
        }
    }
}