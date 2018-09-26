// <copyright file="UserStore.cs">
//    2017 - Johan Boström
// </copyright>

using WasteProducts.IdentityServer.Db;
using WasteProducts.IdentityServer.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.IdentityServer.Stores
{
    public class UserStore :
        UserStore<User, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public UserStore(Context context)
            : base(context)
        {
        }
    }
}