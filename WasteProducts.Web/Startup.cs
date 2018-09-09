using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WasteProducts.Web.Startup))]

namespace WasteProducts.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureSignalR(app);
        }

        private static void ConfigureSignalR(IAppBuilder app)
        {
            var configuration = new HubConfiguration();
#if DEBUG
            configuration.EnableDetailedErrors = true;
#endif
            app.MapSignalR(configuration);
        }
    }
}
