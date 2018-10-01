using Owin;
using WasteProducts.IdentityServer.Extensions;

namespace WasteProducts.Web
{
    public partial class Startup
    {
        private void ConfigureOAuth(IAppBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}