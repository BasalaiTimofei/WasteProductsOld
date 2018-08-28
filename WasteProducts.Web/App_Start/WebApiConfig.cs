using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace WasteProducts.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional},
                new
                {
                    controller = GetControllerNames()
                } // constraint required so this route only matches valid controller names
            );

            config.Routes.MapHttpRoute(
                name: "Search",
                routeTemplate: "api/search/product/{query}",
                defaults: new { controller = "Search", action = "Product"}
            );

            // catch all route mapped to ErrorController so 404 errors
            // can be logged in elmah
            config.Routes.MapHttpRoute(
                "NotFound",
                "api/{*path}",
                new {controller = "Error", action = "NotFound"}
            );
        }

        /// <summary>
        ///     Helper method that returns a string of all api controller names
        ///     in this solution, to be used in route constraints above
        /// </summary>
        /// <returns>constraints pattern</returns>
        private static string GetControllerNames()
        {
            var controllerNames = Assembly.GetCallingAssembly().GetTypes()
                .Where(x => x.IsSubclassOf(typeof(ApiController))).ToList()
                .Select(x => x.Name.Replace("Controller", string.Empty));

            return string.Join("|", controllerNames);
        }
    }
}