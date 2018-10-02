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
                    Flow = Flows.Implicit,

                    ClientId = IdentityConstants.WasteProducts_Front_ClientID,
                    ClientUri = IdentityConstants.WasteProducts_Front_ClientUrl,
                    ClientName = IdentityConstants.WasteProducts_Front_ClientName,

                    RequireConsent = true,
                    AllowRememberConsent = true,

                    RedirectUris = new List<string>
                    {
                        IdentityConstants.WasteProducts_Front_ClientUrl,
                    },

                    PostLogoutRedirectUris = new List<string>
                    {
                        IdentityConstants.WasteProducts_Front_ClientUrl,
                    },

                    AllowedCorsOrigins = new List<string>
                    {
                        IdentityConstants.WasteProducts_Front_ClientUrl,
                    },

                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Email,
                        Constants.StandardScopes.Roles,
                        IdentityConstants.WasteProducts_Api_Scope
                    },

                    // access token settings
                    AccessTokenLifetime = 3600,
                    AccessTokenType = AccessTokenType.Jwt,

                    // refresh token settings
                    AbsoluteRefreshTokenLifetime = 86400,
                    SlidingRefreshTokenLifetime = 43200,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },

                ///////////////////////////////////////////////////////////////
                //// MVC No Library Client
                ///////////////////////////////////////////////////////////////
                //new Client
                //{
                //    ClientName = "OpenID Connect without Client Library Sample",
                //    ClientId = IdentityConstants.WasteProducts_Api_ClientID,
                //    Flow = Flows.Implicit,
                //    ClientSecrets = new List<Secret>()
                //    {
                //        new Secret(IdentityConstants.WasteProducts_Api_Secret.Sha256())
                //    },

                //    AllowedScopes = new List<string>
                //    {
                //        Constants.StandardScopes.OpenId,
                //        Constants.StandardScopes.Profile,
                //        Constants.StandardScopes.Email,
                //        Constants.StandardScopes.Roles
                //    },

                //    ClientUri = "https://identityserver.io",

                //    RequireConsent = true,
                //    AllowRememberConsent = true,

                //    RedirectUris = new List<string>
                //    {
                //        "http://localhost:11716/account/signInCallback",
                //    },
                //},
            };
        }
    }
}