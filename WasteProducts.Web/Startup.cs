using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WasteProducts.Web.Startup))]

namespace WasteProducts.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
