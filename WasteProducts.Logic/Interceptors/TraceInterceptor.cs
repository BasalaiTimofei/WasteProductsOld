using System.Diagnostics;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Logging;
using WasteProducts.Logic.Resources;

namespace WasteProducts.Logic.Interceptors
{
    /// <summary>
    /// Interceptor for trace logging
    /// </summary>
    public class TraceInterceptor : AsyncInterceptor
    {
        private readonly ILogger _logger;
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// Constructor with arguments
        /// </summary>
        /// <param name="logger">NLog logger</param>
        public TraceInterceptor(ILogger logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }


        /// <inheritdoc />
        protected override void BeforeInvoke(IInvocation invocation)
        {
            var methodInfo = invocation.Request.Method;
            _logger.Error(InterceptorResources.TraceStartMessageFormat, 
                methodInfo.DeclaringType?.Name ?? InterceptorResources.NoDeclaringType, 
                methodInfo.Name);

            _stopwatch.Start();
        }

        /// <inheritdoc />
        protected override void AfterInvoke(IInvocation invocation)
        {
            _stopwatch.Stop();

            var methodInfo = invocation.Request.Method;
            _logger.Error(InterceptorResources.TraceEndMessageFormat, 
                methodInfo.DeclaringType?.Name ?? InterceptorResources.NoDeclaringType, 
                methodInfo.Name, 
                _stopwatch.ElapsedMilliseconds);
        }
    }
}