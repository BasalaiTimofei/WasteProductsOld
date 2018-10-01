using System.IO;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using Owin;
using WasteProducts.IdentityServer.Certificate;

namespace WasteProducts.IdentityServer.Extensions
{
    public static class IdentityServerMiddlewareExtension
    {
        public static IAppBuilder UseIdentityServer(this IAppBuilder app, string pathPrefix = "/identity")
        {
            return app.Map(pathPrefix, subApp => {
                subApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Waste Products Identity Server",
                    SigningCertificate = CertificateLoader.Get(),
                    Factory = new IdentityServerServiceFactory().Configure(),

                    RequireSsl = true,
                });

            });
        }
    }
}