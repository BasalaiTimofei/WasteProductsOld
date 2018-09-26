// <copyright file="RoleStore.cs">
//    2017 - Johan Boström
// </copyright>

using WasteProducts.IdentityServer.Db;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.IdentityServer.Stores
{
    public class RoleStore : RoleStore<IdentityRole>
    {
        public RoleStore(Context context)
            : base(context)
        {
        }
    }
}