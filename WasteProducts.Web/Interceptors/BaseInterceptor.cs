using System.Collections.Generic;
using System.Linq;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Request;
using NLog;

namespace WasteProducts.Web.Interceptors
{
    public abstract class BaseInterceptor : IInterceptor
    {
        protected BaseInterceptor(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        public abstract void Intercept(IInvocation invocation);

        protected string GetMethodInfoString(IProxyRequest request)
        {
            var typeName = request.Target.GetType();
            var methodName = request.Method.Name;
            var returnTypeName = request.Method.ReturnType.Name;

            return $"{typeName}.{methodName}:{returnTypeName}";
        }

        protected string GetArgumentsString(object[] args)
        {
            if (args == null || !args.Any())
            {
                return "without arguments";
            }

            return $"with arguments: {string.Join("; ", args)}";
        }
    }
}