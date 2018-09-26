// <copyright file="UserManager.cs">
//    2017 - Johan Boström
// </copyright>

using WasteProducts.IdentityServer.Factories;
using WasteProducts.IdentityServer.Models;
using WasteProducts.IdentityServer.Stores;
using Microsoft.AspNet.Identity;

namespace WasteProducts.IdentityServer.Managers
{
    public class UserManager : UserManager<User, string>
    {
        public UserManager(UserStore store)
            : base(store)
        {
            ClaimsIdentityFactory = new ClaimsIdentityFactory();
        }
    }
}