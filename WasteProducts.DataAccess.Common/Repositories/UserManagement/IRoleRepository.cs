using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories.UserManagement
{
    /// <summary>
    /// Standart DAL level interface using to make CRUD operations with IdentityRole models.
    /// </summary>
    public interface IRoleRepository : IRoleRepository<IdentityRole, string, IdentityUserRole>
    {
    }

    /// <summary>
    /// Standart DAL level interface using to make CRUD operations with TRole models.
    /// </summary>
    public interface IRoleRepository<TRole, TKey, TUserRole>
        where TRole : IdentityRole<TKey, TUserRole>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
    {
        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="role">A new role.</param>
        /// <returns></returns>
        Task CreateAsync(TRole role);

        /// <summary>
        /// Delete a role.
        /// </summary>
        /// <param name="role">Deleting role.</param>
        /// <returns></returns>
        Task DeleteAsync(TRole role);

        /// <summary>
        /// Find a role by id.
        /// </summary>
        /// <param name="roleId">Id of the wanted role.</param>
        /// <returns></returns>
        Task<TRole> FindByIdAsync(TKey roleId);

        /// <summary>
        /// Find a role by name.
        /// </summary>
        /// <param name="roleName">Name of the wanted role.</param>
        /// <returns></returns>
        Task<TRole> FindByNameAsync(string roleName);

        /// <summary>
        /// Update a role.
        /// </summary>
        /// <param name="role">Updating role.</param>
        /// <returns></returns>
        Task UpdateAsync(TRole role);
    }
}
