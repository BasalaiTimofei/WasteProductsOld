using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Ninject;
using Owin;

namespace WasteProducts.Web.Extensions
{
    /// <summary>
    /// Extension methods for IAppBuilder
    /// </summary>
    public static class SignalRExtensions
    {
        /// <summary>
        /// Enables and configures SignalR for owin app
        /// </summary>
        /// <param name="app">Owin app builder</param>
        public static void ConfigureSignalR(this IAppBuilder app)
        {
            var dependencyResolver = new NinjectDependencyResolver(NinjectWebCommon.Bootstrapper.Kernel);

            GlobalHost.DependencyResolver = dependencyResolver;

            var hubConfig = new HubConfiguration
            {
                Resolver = dependencyResolver,
                EnableJavaScriptProxies = false,
                EnableDetailedErrors = true
            };

            app.MapSignalR(hubConfig);
        }
    }
}