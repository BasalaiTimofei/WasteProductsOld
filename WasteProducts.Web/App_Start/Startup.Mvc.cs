using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(WasteProducts.Web.Startup))]

namespace WasteProducts.Web
{
    /// <summary>
    /// Owin Startup file
    /// </summary>
    public partial class Startup
    {
        private void ConfigureMvc()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterMvcRoutes(RouteTable.Routes);
        }

        private void RegisterMvcRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");

            routes.MapRoute(
                "Errors",
                "error/{resource}",
                new { controller = "Error", action = "Index", resource = UrlParameter.Optional });

            routes.MapRoute(
                "Errors.Detail",
                "error/detail/{resource}",
                new { controller = "Error", action = "Detail", resource = UrlParameter.Optional });

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}