using System;
using System.Linq;
using Ninject;
using Ninject.Extensions.Interception.Infrastructure.Language;
using WasteProducts.Logic.Interceptors;

namespace WasteProducts.Logic.Extensions
{
    /// <summary>
    /// Ninject kernel extension methods for interception
    /// </summary>
    public static class KernelInterceptionExtensions
    {
        /// <summary>
        /// Indicates that all interface bindings should be intercepted via the trace interceptor.
        /// The interceptor will be created via the kernel when the method is called.
        /// </summary>
        /// /// <param name="kernel">Ninject kernel</param> 
        public static void TraceInterfaceBindings(this IKernel kernel)
        {
            kernel.Intercept(context => context.Request.Service.IsInterface).With<TraceInterceptor>().InOrder(0);
        }

        /// <summary>
        /// Indicates that matching interface binding types should be intercepted via the trace interceptor.
        /// The interceptor will be created via the kernel when the method is called.
        /// </summary>
        /// <param name="kernel">Ninject kernel</param>
        /// <param name="types">Types of interface</param>
        /// <returns>The advice order builder.</returns>
        public static void TraceInterfaceBindings(this IKernel kernel, params Type[] types)
        {
            kernel.Intercept(context => context.Request.Service.IsInterface && types.Contains(context.Request.Service)).With<TraceInterceptor>().InOrder(0);
        }
    }
}