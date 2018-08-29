using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using WasteProducts.Logic.Common.Models.Search;
using WasteProducts.Web.Utils.Search;

namespace WasteProducts.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var provider = new SimpleModelBinderProvider(typeof(BoostedSearchQuery), new BoostedSearchQueryModelBinder());
            config.Services.Insert(typeof(ModelBinderProvider), 0, provider);

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