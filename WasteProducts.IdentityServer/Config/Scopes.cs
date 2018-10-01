using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace WasteProducts.IdentityServer.Config
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>();

            scopes.AddRange(new[]{
                ////////////////////////
                // identity scopes
                ////////////////////////
                StandardScopes.OpenId,
                StandardScopes.Profile,

                StandardScopes.EmailAlwaysInclude,
                StandardScopes.RolesAlwaysInclude,

                StandardScopes.OfflineAccess,
            });

            scopes.AddRange(new[]
            {
                ////////////////////////
                // resource scopes
                ////////////////////////
                new Scope()
                {
                    Type = ScopeType.Resource,
                    Name = IdentityConstants.WasteProducts_Api_Scope,
                    DisplayName = IdentityConstants.WasteProducts_Api_Name,
                    Description = IdentityConstants.WasteProducts_Api_Description,
                    

                    ScopeSecrets = new List<Secret>
                    {
                        new Secret(IdentityConstants.WasteProducts_Api_Secret.Sha256())
                    }
                },
            });

            return scopes;
        }
    }
}