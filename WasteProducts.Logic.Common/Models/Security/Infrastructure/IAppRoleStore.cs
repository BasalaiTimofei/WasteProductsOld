using Microsoft.AspNet.Identity;
using WasteProducts.Logic.Common.Models.Security.Infrastructure;

namespace WasteProducts.Logic.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the RoleStore. Has an inheritance from Microsoft.AspNet.Identity.IRoleStore.
    /// </summary>
    public interface IAppRoleStore : IRoleStore<IAppRole, int>
    {

    }
}
