using System;

namespace WasteProducts.Logic.Common.Attributes
{
    /// <summary>
    /// Implementations of interfaces marked with this attribute will be wrapped in a proxy with the trace asynchronous interceptor
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class TraceAttribute : Attribute
    {
    }
}