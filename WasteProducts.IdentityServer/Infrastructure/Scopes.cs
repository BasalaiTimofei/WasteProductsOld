using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace WasteProducts.IdentityServer.Infrastructure
{
    public class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new[]
                {
                    ////////////////////////
                    // identity scopes
                    ////////////////////////

                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.Roles,
                    
                    ////////////////////////
                    // resource scopes
                    ////////////////////////

                     new Scope
                    {

                        Name = "wasteproducts-api",
                        DisplayName = "Waste Products Web Api",
                        Description = "Waste Products Web Api",

                        ScopeSecrets = new List<Secret>
                        {
                            new Secret(Constants.secret_api_.Sha256())
                        },

                        Type = ScopeType.Resource
                    },

                    new Scope
                    {
                        Name = "read",
                        DisplayName = "Read data",
                        Type = ScopeType.Resource,
                        Emphasize = false,

                        ScopeSecrets = new List<Secret>
                        {
                            new Secret(Constants.secret_read_.Sha256())
                        }
                    },
                    new Scope
                    {
                        Name = "write",
                        DisplayName = "Write data",
                        Type = ScopeType.Resource,
                        Emphasize = true,

                        ScopeSecrets = new List<Secret>
                        {
                            new Secret(Constants.secret_write_.Sha256())
                        }
                    }
                };
        }
    }
}