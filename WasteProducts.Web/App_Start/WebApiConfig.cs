using System.Linq;
using System.Reflection;
using System.Web.Http;
using WasteProducts.Web.Controllers.Api;

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
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional},
                constraints: new { controller = GetControllerNames() } // constraint required so this route only matches valid controller names
            );

            // catch all route mapped to ErrorController so 404 errors
            // can be logged in elmah
            config.Routes.MapHttpRoute(
                name: "NotFound",
                routeTemplate: "api/{*path}",
                defaults: new { controller = "Error", action = "NotFound" }
            );
        }

        /// <summary>
        /// Helper method that returns a string of all api controller names
        /// in this solution, to be used in route constraints above
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