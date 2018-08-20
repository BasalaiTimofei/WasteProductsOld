using Microsoft.AspNet.Identity;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security.Strores
{
    /// <summary>
    /// Interface for the RoleStore. Has an inheritance from Microsoft.AspNet.Identity.IRoleStore.
    /// </summary>
    public interface IAppRoleStore : IRoleStore<IAppRole, int>
    {

    }
}
