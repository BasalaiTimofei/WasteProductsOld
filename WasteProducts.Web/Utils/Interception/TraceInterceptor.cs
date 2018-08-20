using System.Diagnostics;
using Castle.DynamicProxy;
using NLog;

namespace WasteProducts.Web.Utils.Interception
{
    /// <summary>
    /// Interceptor for trace logging
    /// </summary>
    public class TraceInterceptor : AsyncTimingInterceptor
    {
        private ILogger _logger { get; }

        public TraceInterceptor(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Before invocation method 
        /// </summary>
        /// <param name="invocation"></param>
        protected override void StartingTiming(IInvocation invocation)
        {
            _logger.Trace($"Start {invocation.InvocationTarget.GetType().Name}:{invocation.Method.Name}");
        }

        /// <summary>
        /// After invocation method 
        /// </summary>
        /// <param name="invocation"></param>
        protected override void CompletedTiming(IInvocation invocation, Stopwatch stopwatch)
        {
            _logger.Trace($"End  {invocation.InvocationTarget.GetType().Name}:{invocation.Method.Name} after {stopwatch.ElapsedMilliseconds} milliseconds");
        }
    }
}