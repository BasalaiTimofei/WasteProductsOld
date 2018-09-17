using Owin;
using System.Web.Http;
using WasteProducts.Web.Api;

namespace WasteProducts.Web
{
    public partial class Startup
    {
        private const string ApiPrefix = "api";

        private void ConfigureApi(IAppBuilder app)
        {
            //app.Map('/' + ApiPrefix, appBuilder =>
            //{

            //});
            var configuration = new HttpConfiguration();
            configuration.ConfigureSwagger(ApiPrefix);

            app.ConfigureSignalR(ApiPrefix);

            app.ConfigureWebApi(configuration, ApiPrefix);
            
        }
    }
}