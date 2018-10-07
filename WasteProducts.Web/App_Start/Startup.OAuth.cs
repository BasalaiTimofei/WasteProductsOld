using IdentityServer3.AccessTokenValidation;
using Owin;
using Serilog;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;
using WasteProducts.IdentityServer;
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
                Authority = "https://waste-api.belpyro.net/identity",
                RequiredScopes = new[] { IdentityConstants.WasteProducts_Api_Scope },
                SigningCertificate = CertificateLoader.Load(),
                ValidationMode = ValidationMode.ValidationEndpoint
            });

            app.UseIdentityServer();
            app.UseResourceAuthorization(new AuthorizationManager());
        }
    }

    public partial class Startup
    {
        //private void ConfigureOAuth(IAppBuilder app)
        //{

        //    app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
        //    {
        //        Authority = "https://localhost:44378/identity",
        //        RequiredScopes = new[] { IdentityConstants.WasteProducts_Api_Scope },
        //        SigningCertificate = CertificateLoader.Load(),
        //        ValidationMode = ValidationMode.ValidationEndpoint
        //    });

        //    app.UseIdentityServer();
        //    app.UseResourceAuthorization(new AuthorizationManager());
        //}
    }

    public class AuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            switch (context.Resource.First().Value)
            {
                case "ContactDetails":
                    return AuthorizeContactDetails(context);
                default:
                    return Nok();
            }
        }

        private Task<bool> AuthorizeContactDetails(ResourceAuthorizationContext context)
        {
            switch (context.Action.First().Value)
            {
                case "wasteproducts-api":
                    return Eval(context.Principal.HasClaim(IdentityConstants.WasteProducts_Api_Scope, IdentityConstants.WasteProducts_Api_Scope));
                default:
                    return Nok();
            }
        }
    }

}