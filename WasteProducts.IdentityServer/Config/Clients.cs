using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace WasteProducts.IdentityServer.Config
{
    public static class Clients
    {
        public static IEnumerable<Client> Load()
        {
            return new List<Client>
            {
                /////////////////////////////////////////////////////////////
                // JavaScript Implicit Client - TokenManager
                /////////////////////////////////////////////////////////////
                  new Client
                {
                    //ClientName = "Waste Products Angular Client",
                    //ClientId = "wasteproducts.front.angular",
                    ClientId = IdentityConstants.WasteProducts_Front_ClientID,
                    ClientUri = IdentityConstants.WasteProducts_Front_ClientUrl,
                    ClientName = IdentityConstants.WasteProducts_Front_ClientName,
                    Flow = Flows.ResourceOwner,

                    RequireConsent = true,
                    AllowRememberConsent = true,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret(IdentityConstants.WasteProducts_Client_Secret.Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Email,
                        Constants.StandardScopes.Roles,
                       IdentityConstants.WasteProducts_Api_Scope
                    },

                    RedirectUris = new List<string>
                    {
                        IdentityConstants.WasteProducts_Front_ClientUrl
                    },

                    PostLogoutRedirectUris = new List<string>
                    {
                        IdentityConstants.WasteProducts_Front_ClientUrl
                    },

                    // used by JS resource owner sample
                    AllowedCorsOrigins = new List<string>
                    {
                       IdentityConstants.WasteProducts_Front_ClientUrl
                    },

                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 3600,
                    AuthorizationCodeLifetime = 86400,
                    IdentityTokenLifetime = 86400,

                    // refresh token settings
                    AbsoluteRefreshTokenLifetime = 86400,
                    SlidingRefreshTokenLifetime = 43200,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },
            };
        }
    }
}