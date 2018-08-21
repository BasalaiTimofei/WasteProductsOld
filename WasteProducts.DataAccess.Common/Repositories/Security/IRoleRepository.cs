using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    /// <summary>
    /// Interface IRoleRepository for the RoleRepository.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Finding Role by role name
        /// </summary>
        /// <param name="roleName">search by name</param>
        /// <returns>IRoleDb object</returns>
        IRoleDb FindByName(string roleName);
        /// <summary>
        /// Finding Role by role name async with CancellationToken
        /// </summary>
        /// <param name="roleName">search by name</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>IRoleDb as Task</returns>
        Task<IRoleDb> FindByNameAsync(CancellationToken cancellationToken, string roleName);

        /// <summary>
        /// Finding Role by role name async
        /// </summary>
        /// <param name="name">search by name</param>
        /// <returns>IRoleDb as Task</returns>
        Task<IRoleDb> FindByNameAsync(string name);

        /// <summary>
        /// Finding Role name by user id
        /// </summary>
        /// <param name="userId">search role list by user id</param>
        /// <returns>List of strings as Task</returns>
        Task<List<string>> GetRolesNameByUserId(int userId);
    }
}