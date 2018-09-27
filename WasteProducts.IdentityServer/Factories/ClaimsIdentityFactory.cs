// <copyright file="ClaimsIdentityFactory.cs">
//    2017 - Johan Boström
// </copyright>

using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.IdentityServer.Models;
using Microsoft.AspNet.Identity;

namespace WasteProducts.IdentityServer.Factories
{
    public class ClaimsIdentityFactory : ClaimsIdentityFactory<User, string>
    {
        public ClaimsIdentityFactory()
        {
            UserIdClaimType = IdentityServer3.Core.Constants.ClaimTypes.Subject;
            UserNameClaimType = IdentityServer3.Core.Constants.ClaimTypes.PreferredUserName;
            RoleClaimType = IdentityServer3.Core.Constants.ClaimTypes.Role;
        }

        public override async Task<ClaimsIdentity> CreateAsync(UserManager<User, string> manager, User user,
            string authenticationType)
        {
            var ci = await base.CreateAsync(manager, user, authenticationType);

            if (!string.IsNullOrWhiteSpace(user.FirstName))
                ci.AddClaim(new Claim("given_name", user.FirstName));

            if (!string.IsNullOrWhiteSpace(user.LastName))
                ci.AddClaim(new Claim("family_name", user.LastName));

            return ci;
        }
    }
}