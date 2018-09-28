using WasteProducts.IdentityServer.Stores;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.IdentityServer.Managers
{
    public class RoleManager : RoleManager<IdentityRole>
    {
        public RoleManager(RoleStore store)
            : base(store)
        {
        }
    }
}