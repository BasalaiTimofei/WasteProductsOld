using Owin;
using System.Web.Http;
using WasteProducts.Web.Extensions;

namespace WasteProducts.Web
{
    public partial class Startup
    {
        private void ConfigureApi(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.ConfigureSwagger();

            app.ConfigureSignalR();

            app.ConfigureWebApi(configuration);
            
        }
    }
}