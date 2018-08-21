using Microsoft.AspNet.Identity;
using System;
using WasteProducts.Logic.Common.Models.Security.Infrastructure;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Services
{
    /// <summary>
    /// App User Manager Service has an inheritance from Microsoft.AspNet.Identity.UserManager. Security model class
    /// </summary>
    public class AppUserManagerService : UserManager<IAppUser, int>
    {
        public AppUserManagerService(IAppUserStore store) : base(store)
        {

        }
    }
}
