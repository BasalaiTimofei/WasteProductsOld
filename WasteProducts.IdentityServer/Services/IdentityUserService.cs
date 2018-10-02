using IdentityServer3.AspNetIdentity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.IdentityServer.Services
{
    public class IdentityUserService : AspNetIdentityUserService<UserDB, string>
    {
        public IdentityUserService(UserManager userManager) : base(userManager)
        {
        }
    }


    public class UserStore : UserStore<UserDB>
    {
        public UserStore(WasteContext context) : base(context) { }
    }
    
    public class UserManager : UserManager<UserDB>
    {
        public UserManager(UserStore userStore)
            : base(userStore)
        {
        }
    }

    //public class RoleStore : RoleStore<IdentityRole>
    //{
    //    public RoleStore(WasteContext context) : base(context) { }
    //}

    //public class RoleManager : RoleManager<IdentityRole>
    //{
    //    public RoleManager(RoleStore roleStore) : base(roleStore) { }
    //}

}