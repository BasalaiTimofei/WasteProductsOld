using WasteProducts.DataAccess.Common.Repositories.Security;
using System.Data.Entity;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Repositories.Security
{
    /// <summary>
    /// UserRole repository class inheritance from RepositoryBase T and IUserRoleRepository interface
    /// </summary>
    internal class UserRoleRepository : RepositoryBase<IUserRoleDb>, IUserRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of UserRoleRepository
        /// </summary>
        /// <param name="dbFactory">Used to set DbFactory in base class RepositoryBase </param>
        public UserRoleRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Checking if user with current id has role with current id role
        /// </summary>
        /// <param name="userId">user id number</param>
        /// <param name="roleId">role id number</param>
        /// <returns>bool value</returns>
        public Task<bool> IsInRoleAsync(int userId, int roleId)
        {
            return _dbSet.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }
    }
}
