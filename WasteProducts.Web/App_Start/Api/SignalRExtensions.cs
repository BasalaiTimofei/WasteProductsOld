using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Ninject;
using Owin;

namespace WasteProducts.Web.Api
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
        /// <param name="pathPrefix">route prefix</param>
        public static void ConfigureSignalR(this IAppBuilder app, string pathPrefix = "")
        {
            var dependencyResolver = new NinjectDependencyResolver(NinjectWebCommon.Bootstrapper.Kernel);

            var hubConfig = new HubConfiguration
            {
                Resolver = dependencyResolver,
                EnableJavaScriptProxies = false,
                EnableDetailedErrors = true
            };

            app.MapSignalR('/' + pathPrefix + "/hubs", hubConfig);
        }
    }
}