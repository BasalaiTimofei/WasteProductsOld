using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;

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
                    ClientName = "Angular Client",
                    ClientId = "bc04b7e1-f43b-46c0-9908-3c2fe4633987",
                    Flow = Flows.ResourceOwner,

                     ClientSecrets = new List<Secret>
                   {
                       new Secret(Constants.secretApi_angular.Sha256())
                   },

                    ClientUri = @"http://localhost:4200/index.html",

                    RedirectUris = new List<string>
                    {
                        "https://localhost:4300"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:4200/index.html"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:4200"
                    },
                    AllowedScopes = new List<string>()
                    { "openid", "profile" , "roles",  "read", "write", "Waste Products Web Api" }
                     ,
                     AccessTokenType = AccessTokenType.Jwt,
                     IdentityTokenLifetime = 3600,
                     AccessTokenLifetime = 3600,

                     // refresh token settings
                     AbsoluteRefreshTokenLifetime = 86400,
                     SlidingRefreshTokenLifetime = 43200,
                     RefreshTokenUsage = TokenUsage.OneTimeOnly,
                     RefreshTokenExpiration = TokenExpiration.Sliding

                 }
                 ,

               new Client
               {
                   ClientName = "WebAPI Client",
                   ClientId = "f42b18de-ed39-4d61-af90-40385bb8b34f",
                   Flow = Flows.ResourceOwner,
                   ClientSecrets = new List<Secret>
                   {
                       new Secret(Constants.secretApi_webApi.Sha256())
                   },
                   //AllowAccessToAllScopes = true,
                   AllowedScopes = new List<string>()
                   { "openid", "profile" , "roles",  "read", "write" }
                   ,

                   ClientUri = @"http://localhost:2189/",

                    RedirectUris = new List<string>
                   {
                      "https://localhost:4300"
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