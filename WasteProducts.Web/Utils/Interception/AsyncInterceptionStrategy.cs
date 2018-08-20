using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            if (!targetType.IsInterface)
                return;

            IAsyncInterceptor[] interceptors = GetInterceptors(targetType, context.Kernel);

            if (interceptors.Any())
            {
                reference.Instance = _generator.CreateInterfaceProxyWithTargetInterface(targetType, reference.Instance, ProxyOptions, interceptors);
            }
        }

        /// <summary>
        /// Looks for interceptor attributes, and gets interceptors from ninject kernel
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="kernel"></param>
        /// <returns></returns>
        private IAsyncInterceptor[] GetInterceptors(Type targetType, IKernel kernel)
        {
            var interceptors = new List<IAsyncInterceptor>();

            if (targetType.GetCustomAttribute<TraceAttribute>(true) != null && kernel.CanResolve<TraceInterceptor>())
            {
                interceptors.Add(kernel.Get<TraceInterceptor>());
            }

            return interceptors.ToArray();
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