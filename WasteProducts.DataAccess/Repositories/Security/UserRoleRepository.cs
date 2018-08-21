using WasteProducts.DataAccess.Common.Repositories.Security;
using System.Data.Entity;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Repositories.Security
{
    internal class UserRoleRepository : RepositoryBase<IUserRoleDb>, IUserRoleRepository
    {
        public UserRoleRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public Task<bool> IsInRoleAsync(int userId, int roleId)
        {
            return dbSet.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }
    }
}
