using System.Linq;
using System.Web.Http.ExceptionHandling;
using Ninject.Web.Mvc.FilterBindingSyntax;
using NLog.Web;
using WasteProducts.Web.Controllers;
//using WasteProducts.Web.Utils;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WasteProducts.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WasteProducts.Web.App_Start.NinjectWebCommon), "Stop")]

namespace WasteProducts.Web.App_Start
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.WebApi.FilterBindingSyntax;
    using NLog;
    using WasteProducts.Web.Filters;
    using WasteProducts.Web.Interceptors;

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
            LogManager.Flush();
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
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                
                RegisterLoggers(kernel);
                RegisterFiltres(kernel);
                RegisterServices(kernel);

                return kernel;
            }
            catch (Exception e)
            {
                kernel.TryGet<ILogger>()?.Fatal(e, "Kernel throw exception during the creation process.");

                LogManager.Flush();

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
            kernel.BindFilter<MvcExceptionFilterAttribute>(System.Web.Mvc.FilterScope.First, 0);
            kernel.BindHttpFilter<ApiExceptionFilterAttribute>(System.Web.Http.Filters.FilterScope.Global);
        }

		/// <summary>
        /// Register your loggers here
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterLoggers(IKernel kernel)
        {
            kernel.Bind<ILogger>().ToMethod(ctx =>
            {
                var name = ctx.Request.Target?.Member.DeclaringType.Name ?? "GlobalLogger";
                return LogManager.GetLogger(name);
            });
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //test interception
            //var b = kernel.Bind<ITestObj>().To<TestObj>();
            //b.Intercept().With<LogInterceptor>(); 
            //b.Intercept().With<TimerInterceptor>();
        }
    }
}