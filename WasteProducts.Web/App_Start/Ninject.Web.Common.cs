using WasteProducts.Web.Controllers.Api;
using Ninject.Extensions.Interception.Infrastructure.Language;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WasteProducts.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(WasteProducts.Web.App_Start.NinjectWebCommon), "Stop")]

namespace WasteProducts.Web.App_Start
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Http.ExceptionHandling;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Activation.Strategies;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using NLog;

    using WasteProducts.Web.Utils.Interception;
    using WasteProducts.Web.Filters;
    using Microsoft.AspNet.SignalR;
    using WasteProducts.Web.Utils;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            LogManager.Shutdown();
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Components.Add<IActivationStrategy, AsyncInterceptionStrategy>();

                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                GlobalHost.DependencyResolver = new NinjectSignalRDependencyResolver(kernel);

                RegisterLoggers(kernel);
                RegisterFiltres(kernel);
                RegisterServices(kernel);

                return kernel;
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Fatal(e, "Kernel throw exception during the creation process.");
                LogManager.Shutdown();

                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Register your filtres here
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterFiltres(IKernel kernel)
        {
            kernel.BindFilter<MvcUnhandledExceptionFilter>(FilterScope.Global, 0);
        }

		/// <summary>
        /// Register your loggers here
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterLoggers(IKernel kernel)
        {
            kernel.Bind<ILogger>().ToMethod(ctx =>
            {
                var targetName = ctx.Request.Target?.Member.DeclaringType.FullName ?? "GlobalLogger";
                return LogManager.GetLogger(targetName);
            });
            kernel.Bind<IExceptionLogger>().To<ApiUnhandledExceptionLogger>();
            kernel.Bind<TraceInterceptor>().ToSelf();
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load("WasteProducts.DataAccess.dll");
            kernel.Load("WasteProducts.Logic.dll");
        }
    }
}