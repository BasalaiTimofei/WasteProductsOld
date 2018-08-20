using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.DynamicProxy;
using Microsoft.CSharp.RuntimeBinder;
using Ninject;
using Ninject.Activation;
using Ninject.Activation.Strategies;
using WasteProducts.Logic.Common.Attributes;

namespace WasteProducts.Web.Utils.Interception
{
    /// <summary>
    /// Ninject dynamic proxy strategy
    /// </summary>
    public class AsyncInterceptionStrategy : IActivationStrategy
    {
        private static readonly ProxyGenerationOptions ProxyOptions = ProxyGenerationOptions.Default;
        private ProxyGenerator _generator = new ProxyGenerator();

        /// <summary>
        /// Ninject Settings
        /// </summary>
        public INinjectSettings Settings { get; set; }

        /// <summary>
        /// Releases all resources held by the object.
        /// </summary>
        public void Dispose()
        {
            _generator = null;
        }

        /// <summary>
        /// Activation strategy
        /// </summary>
        /// <param name="context">injection context</param>
        /// <param name="reference">injection target reference</param>
        public void Activate(IContext context, InstanceReference reference)
        {
            WrapToProxyObj(context, reference);
        }

        /// <summary>
        /// Deactivate strategy
        /// </summary>
        /// <param name="context">injection context</param>
        /// <param name="reference">injection target reference</param>
        public void Deactivate(IContext context, InstanceReference reference)
        {
            UnWrapFromProxyObj(context, reference);
        }

        /// <summary>
        /// Wrap injection object into interface based proxy
        /// </summary>
        /// <param name="context">injection context</param>
        /// <param name="reference">injection target reference</param>
        private void WrapToProxyObj(IContext context, InstanceReference reference)
        {
            if (reference.Instance is IInterceptor
                || reference.Instance is IAsyncInterceptor
                || reference.Instance is IProxyTargetAccessor)
            {
                return;
            }

            Type targetType = context.Request.Service;

            var interceptorTypes = targetType
                .GetCustomAttributes(typeof(AsyncInterceptionAttribute), true)
                .Cast<AsyncInterceptionAttribute>()
                .Select(attribute => attribute.Type)
                .Distinct();

            List<IAsyncInterceptor> interceptors = interceptorTypes
                .Select(type => (IAsyncInterceptor)context.Kernel.Get(type))
                .ToList();

            if (targetType.GetCustomAttribute<TraceAttribute>() != null && context.Kernel.CanResolve<TraceInterceptor>())
                interceptors.Add(context.Kernel.Get<TraceInterceptor>());

            if (!interceptors.Any())
                return;

            if (targetType.IsInterface)
                reference.Instance = _generator.CreateInterfaceProxyWithTargetInterface(targetType, reference.Instance, ProxyOptions, interceptors.ToArray());
        }

        /// <summary>
        /// Unwrap object from proxy
        /// </summary>
        /// <param name="context">injection context</param>
        /// <param name="reference">injection target reference</param>
        private void UnWrapFromProxyObj(IContext context, InstanceReference reference)
        {
            if (!ProxyUtil.IsProxy(reference.Instance))
                return;

            try
            {
                dynamic dynamicProxy = reference.Instance;
                if (dynamicProxy != null)
                    reference.Instance = dynamicProxy.__target;
            }
            catch (RuntimeBinderException)
            {
                return;
            }
        }
    }
}