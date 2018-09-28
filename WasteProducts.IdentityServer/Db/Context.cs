// <copyright file="Context.cs">
//    2017 - Johan Boström
// </copyright>

using WasteProducts.IdentityServer.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.IdentityServer.Db
{
    public class Context :
        IdentityDbContext<User, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public Context(string connString)
            : base(connString)
        {
        }
    }
}