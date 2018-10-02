using IdentityServer3.AccessTokenValidation;
using Owin;
using WasteProducts.IdentityServer;
using WasteProducts.IdentityServer.Certificate;
using WasteProducts.IdentityServer.Extensions;

namespace WasteProducts.Web
{
    public partial class Startup
    {
        private void ConfigureOAuth(IAppBuilder app)
        {
            app.UseIdentityServer();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                IssuerName = "issuerName",
                SigningCertificate = CertificateLoader.Load(),
                Authority = "https://localhost:44362/identity",
                RequiredScopes = new[] { IdentityConstants.WasteProducts_Api_Scope },

                //ClientId = IdentityConstants.WasteProducts_Api_ClientID,
                //ClientSecret = IdentityConstants.WasteProducts_Api_Secret,



            });
        }
    }
}