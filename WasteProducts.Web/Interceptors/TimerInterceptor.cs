using System.Diagnostics;
using Ninject.Extensions.Interception;
using NLog;

namespace WasteProducts.Web.Interceptors
{
    public class TimerInterceptor : BaseInterceptor
    {
        public TimerInterceptor(ILogger logger) : base(logger) { }

        public override void Intercept(IInvocation invocation)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                invocation.Proceed();
            }
            finally
            {
                watch.Stop();

                var methodInfo = GetMethodInfoString(invocation.Request);

                Logger.Info($"Method: {methodInfo} returned after: {watch.ElapsedMilliseconds} milliseconds");
            }
        }
    }
}