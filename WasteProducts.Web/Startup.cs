﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WasteProducts.Web.Startup))]

namespace WasteProducts.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMvc();

            ConfigureCors(app);
            ConfigureOAuth(app);

            ConfigureApi(app);
        }
    }
}
