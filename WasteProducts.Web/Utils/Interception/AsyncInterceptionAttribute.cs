using System;

namespace WasteProducts.Web.Utils.Interception
{
    /// <summary>
    /// Implementations of interfaces marked with this attribute will be wrapped in a proxy with the specified asynchronous interceptor
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class AsyncInterceptionAttribute : Attribute
    {
        public Type Type { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="asyncInterceptorType">Type of asynchronous interceptor</param>
        public AsyncInterceptionAttribute(Type asyncInterceptorType)
        {
            Type = asyncInterceptorType;
        }
    }
}