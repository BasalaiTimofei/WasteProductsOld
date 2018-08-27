using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Services.UserService
{
    /// <summary>
    /// Standart BL level interface that exposes basic role management.
    /// </summary>
    public interface IUserRoleService
    {
        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="role">A new role.</param>
        /// <returns></returns>
        Task CreateAsync(UserRole role);

        /// <summary>
        /// Delete a role.
        /// </summary>
        /// <param name="role">Deleting role.</param>
        /// <returns></returns>
        Task DeleteAsync(UserRole role);

        /// <summary>
        /// Find a role by id.
        /// </summary>
        /// <param name="roleId">Id of the wanted role.</param>
        /// <returns></returns>
        Task<UserRole> FindByIdAsync(string roleId);

        /// <summary>
        /// Find a role by name.
        /// </summary>
        /// <param name="roleName">Name of the wanted role.</param>
        /// <returns></returns>
        Task<UserRole> FindByNameAsync(string roleName);

        /// <summary>
        /// Update a role's name.
        /// </summary>
        /// <param name="role">Updating role.</param>
        /// <returns></returns>
        Task UpdateRoleNameAsync(UserRole role, string newRoleName);

        /// <summary>
        /// Returns all users of this role.
        /// </summary>
        /// <param name="role">Users of this role will be returned</param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetRoleUsers(UserRole role);
    }
}
