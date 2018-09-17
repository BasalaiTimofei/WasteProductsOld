using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Elmah;
using Ninject.Extensions.Logging;

namespace WasteProducts.Web.ExceptionHandling.Mvc
{
    /// <summary>
    /// Mvc an attribute that is used to handle all unhandled exceptions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class MvUnhandledExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor with arguments
        /// </summary>
        /// <param name="logger">Ninject logger</param>
        public MvUnhandledExceptionFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public virtual void OnException(ExceptionContext context)
        {
            //Ninject logging
            _logger.Error(context.Exception.Demystify(), string.Empty);

            //Elmah logging
            if (!context.ExceptionHandled || TryRaiseErrorSignal(context) || IsFiltered(context))
                return;

            LogException(context);
        }

        private bool TryRaiseErrorSignal(ExceptionContext context)
        {
            HttpContext httpContextImpl = GetHttpContextImpl(context.HttpContext);

            if (httpContextImpl == null)
                return false;

            ErrorSignal errorSignal = ErrorSignal.FromContext(httpContextImpl);

            if (errorSignal == null)
                return false;

            errorSignal.Raise(context.Exception.Demystify(), httpContextImpl);

            return true;
        }

        private bool IsFiltered(ExceptionContext context)
        {
            ErrorFilterConfiguration section = context.HttpContext.GetSection("elmah/errorFilter") as ErrorFilterConfiguration;

            if (section == null)
                return false;

            ErrorFilterModule.AssertionHelperContext assertionHelperContext = new ErrorFilterModule.AssertionHelperContext(context.Exception, GetHttpContextImpl(context.HttpContext));
            return section.Assertion.Test(assertionHelperContext);
        }

        private void LogException(ExceptionContext context)
        {
            HttpContext httpContextImpl = GetHttpContextImpl(context.HttpContext);
            Error error = new Error(context.Exception, httpContextImpl);
            ErrorLog.GetDefault(httpContextImpl).Log(error);
        }

        private HttpContext GetHttpContextImpl(HttpContextBase context)
        {
            return context.ApplicationInstance.Context;
        }
    }
}