using System.Web.Http;
using Ninject.Web.WebApi;
using Owin;

namespace WasteProducts.Web.Api
{
    /// <summary>
    /// Extension methods for IAppBuilder
    /// </summary>
    public static class WebApiExtensions
    {
        /// <summary>
        /// Enables and configures WebApi for owin app
        /// </summary>
        /// <param name="app">Owin app builder</param>
        /// <param name="configuration">http configuration</param>
        /// <param name="pathPrefix">route prefix</param>
        public static void ConfigureWebApi(this IAppBuilder app, HttpConfiguration configuration, string pathPrefix = "")
        {
            configuration.DependencyResolver = new NinjectDependencyResolver(NinjectWebCommon.Bootstrapper.Kernel);

            RegisterWebApiRoutes(configuration, pathPrefix);

            app.UseWebApi(configuration);
        }

        private static void RegisterWebApiRoutes(HttpConfiguration configuration, string pathPrefix = "")
        {
            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute(
                "DefaultApi",
                pathPrefix + "/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );
        }
    }
}