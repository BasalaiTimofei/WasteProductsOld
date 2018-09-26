using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace WasteProducts.IdentityServer.Infrastructure
{
    public static class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    Enabled = true,
                    Flow = Flows.Implicit,

                    ClientName = "Angular Client",
                    ClientId = "bc04b7e1-f43b-46c0-9908-3c2fe4633987",
                    ClientSecrets = new List<Secret>
                    {
                       new Secret(Constants.secretApi_angular.Sha256())
                    },
                    ClientUri = "http://localhost:4200/",

                    RedirectUris = new List<string>
                    {
                        "http://localhost:4200/",
                        "http://localhost:4200/index.html"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:4200/"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:4200/"
                    },
                    AllowedScopes = new List<string>(){ "openid", "profile" , "roles",  "read", "write", "wasteproducts-api" },
                    AccessTokenType = AccessTokenType.Jwt,
                    IdentityTokenLifetime = 3600,
                    AccessTokenLifetime = 3600,
                    
                    // refresh token settings
                    AbsoluteRefreshTokenLifetime = 86400,
                    SlidingRefreshTokenLifetime = 43200,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },
                new Client
                {
                    Enabled = true,
                    Flow = Flows.ResourceOwner, //TODO: я тут не уверен, скоррее Flows.Implicit,

                    ClientName = "WebAPI Client",
                    ClientId = "f42b18de-ed39-4d61-af90-40385bb8b34f",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(Constants.secretApi_webApi.Sha256())
                    },
                    ClientUri = "http://localhost:2189/api/",

                    //AllowAccessToAllScopes = true,
                    AllowedScopes = new List<string>(){ "openid", "profile" , "roles",  "read", "write" },

                    RedirectUris = new List<string>
                    {
                        "http://localhost:2189/api/"
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    IdentityTokenLifetime = 3600,
                    AccessTokenLifetime = 3600,

                    // refresh token settings
                    AbsoluteRefreshTokenLifetime = 86400,
                    SlidingRefreshTokenLifetime = 43200,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                }
            };
        }
    }
}