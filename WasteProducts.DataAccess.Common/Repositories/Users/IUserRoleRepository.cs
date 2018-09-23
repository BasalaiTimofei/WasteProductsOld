using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Repositories.Users
{
    /// <summary>
    /// Standart DAL level interface using to make CRUD operations with UserRoleDB models.
    /// </summary>
    public interface IUserRoleRepository : IDisposable
    {
        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="role">A new role.</param>
        /// <returns></returns>
        Task AddAsync(UserRoleDB role);

        /// <summary>
        /// Delete a role.
        /// </summary>
        /// <param name="role">Deleting role.</param>
        /// <returns></returns>
        Task DeleteAsync(UserRoleDB role);

        /// <summary>
        /// Find a role by id.
        /// </summary>
        /// <param name="roleId">Id of the wanted role.</param>
        /// <returns></returns>
        Task<UserRoleDB> FindByIdAsync(string roleId);

        /// <summary>
        /// Find a role by name.
        /// </summary>
        /// <param name="roleName">Name of the wanted role.</param>
        /// <returns></returns>
        Task<UserRoleDB> FindByNameAsync(string roleName);

        /// <summary>
        /// Update a role's name.
        /// </summary>
        /// <param name="role">Updating role.</param>
        /// <returns></returns>
        Task UpdateRoleNameAsync(UserRoleDB role);

        /// <summary>
        /// Returns all users of this role.
        /// </summary>
        /// <param name="role">Users of this role will be returned</param>
        /// <returns></returns>
        Task<IEnumerable<UserDB>> GetRoleUsers(UserRoleDB role);
    }
}
