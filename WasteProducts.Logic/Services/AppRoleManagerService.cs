using Microsoft.AspNet.Identity;
using WasteProducts.Logic.Common.Models.Security.Infrastructure;

namespace WasteProducts.Logic.Services
{
    /// <summary>
    /// App Role Manager Service has an inheritance from Microsoft.AspNet.Identity.RoleManager. Security model class
    /// </summary>
    public class AppRoleManagerService : RoleManager<IAppRole,int>
    {
        public AppRoleManagerService(IAppRoleStore store) : base(store)
        {

        }
    }
}
