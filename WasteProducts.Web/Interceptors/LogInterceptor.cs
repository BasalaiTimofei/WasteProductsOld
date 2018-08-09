using System;
using Ninject.Extensions.Interception;
using NLog;

namespace WasteProducts.Web.Interceptors
{
    public class LogInterceptor : BaseInterceptor
    {
        public LogInterceptor(ILogger logger) : base(logger) { }

        public override void Intercept(IInvocation invocation)
        {
            try
            {
                BeforeInvoke(invocation);

                invocation.Proceed();

                AfterInvoke(invocation);
            }
            catch (Exception exception)
            {
                IfThrowException(invocation, exception);
                throw;
            }
        }

        protected void BeforeInvoke(IInvocation invocation)
        {
            var methodInfo = GetMethodInfoString(invocation.Request);
            var arguments = GetArgumentsString(invocation.Request.Arguments);

            Logger.Trace($"Method: {methodInfo} called {arguments}");
        }

        private void IfThrowException(IInvocation invocation, Exception exception)
        {
            var methodInfo = GetMethodInfoString(invocation.Request);
            var arguments = GetArgumentsString(invocation.Request.Arguments);

            Logger?.Error($"Error at Method: {methodInfo} with arguments: {arguments}");
        }

        protected void AfterInvoke(IInvocation invocation)
        {
            var methodInfo = GetMethodInfoString(invocation.Request);

            Logger?.Trace($"Method: {methodInfo} returned {invocation.ReturnValue}");
        }
    }
}