using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;
using WasteProducts.DataAccess.Common.Repositories.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    /// <summary>
    /// Role repository class inheritance from RepositoryBase T and IRoleRepository interface
    /// </summary>
    internal class RoleRepository : RepositoryBase<IRoleDb>, IRoleRepository
    {
        public RoleRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Finding Role by role name
        /// </summary>
        /// <param name="roleName">search by name</param>
        /// <returns>IRoleDb</returns>
        public IRoleDb FindByName(string roleName)
        {
            return _dbSet.FirstOrDefault(x => x.Name == roleName);
        }

        /// <summary>
        /// Finding Role by name async
        /// </summary>
        /// <param name="name">search by name</param>
        /// <returns>IRoleDb as Task</returns>
        public async Task<IRoleDb> FindByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Name.ToUpper() == name.ToUpper());
        }

        /// <summary>
        /// Finding Role by name async with cancellationToken
        /// </summary>
        /// <param name="name">search by name</param>
        /// <returns>IRoleDb as Task</returns>
        public Task<IRoleDb> FindByNameAsync(System.Threading.CancellationToken cancellationToken, string roleName)
        {
            return _dbSet.FirstOrDefaultAsync(x => x.Name.ToUpper() == roleName.ToUpper(), cancellationToken);
        }

        /// <summary>
        /// Finding Role name by user id
        /// </summary>
        /// <param name="userId">search role list by user id</param>
        /// <returns>List of strings as Task</returns>
        public async Task<List<string>> GetRolesNameByUserId(int userId)
        {
            var userRoles = _db.Set<IUserRoleDb>();
            return await  (from role in _dbSet
                           join user in userRoles on role.Id equals user.RoleId
                           where user.UserId == userId
                           select role.Name).ToListAsync();

        }

    }

}
