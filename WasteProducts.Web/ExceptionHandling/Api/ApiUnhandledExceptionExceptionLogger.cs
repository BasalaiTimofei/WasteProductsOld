using System.Diagnostics;
using System.Web.Http.ExceptionHandling;
using Elmah.Contrib.WebApi.Demystifier;
using Ninject.Extensions.Logging;

namespace WasteProducts.Web.ExceptionHandling.Api
{
    /// <summary>
    /// WebApi exception logger with Ninject and Elmah.
    /// </summary>
    public class ApiUnhandledExceptionExceptionLogger : ElmahDemystifierExceptionLogger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor with arguments
        /// </summary>
        /// <param name="logger">Ninject logger</param>
        public ApiUnhandledExceptionExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override void Log(ExceptionLoggerContext context)
        {
            //Ninject logging
            _logger.Error(context.Exception.Demystify(), string.Empty);

            //Elmah logging
            base.Log(context);
        }
    }
}