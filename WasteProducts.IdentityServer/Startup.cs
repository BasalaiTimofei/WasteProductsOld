using WasteProducts.IdentityServer.Config;
using WasteProducts.IdentityServer.Extensions;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WasteProducts.IdentityServer.Startup))]

namespace WasteProducts.IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var certificate = Certificate.Get();
            app.MapCore(certificate);
        }
    }
}