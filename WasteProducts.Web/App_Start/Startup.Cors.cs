using Microsoft.Owin.Cors;
using Owin;

namespace WasteProducts.Web
{
    public partial class Startup
    {
        private void ConfigureCors(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
        }
    }
}