using IdentityServer3.AccessTokenValidation;
using Owin;
using WasteProducts.IdentityServer.Certificate;
using WasteProducts.IdentityServer.Extensions;

namespace WasteProducts.Web
{
    public partial class Startup
    {
        private void ConfigureOAuth(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44362/identity",
                //Authority = "https://waste-api.belpyro.net/identity",
                RequiredScopes = new[] { IdentityConstants.WasteProducts_Api_Scope },
                SigningCertificate = CertificateLoader.Load(),
                ValidationMode = ValidationMode.ValidationEndpoint
            });

            app.UseIdentityServer();
        }
    }
}