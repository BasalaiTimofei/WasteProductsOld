using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using NLog;

namespace WasteProducts.Web.Utils.Interception
{
    public class TraceInterceptor : AsyncTimingInterceptor
    {
        private ILogger _logger { get; }

        protected TraceInterceptor(ILogger logger)
        {
            _logger = logger;
        }

        protected override void StartingTiming(IInvocation invocation)
        {
            _logger.Trace($"Start {invocation.InvocationTarget.GetType().Name}:{invocation.Method.Name}");
        }

        protected override void CompletedTiming(IInvocation invocation, Stopwatch stopwatch)
        {
            _logger.Trace($"End  {invocation.InvocationTarget.GetType().Name}:{invocation.Method.Name} after {stopwatch.ElapsedMilliseconds} milliseconds");
        }
    }
}