using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Repositories.Security
{
    internal class RoleRepository : RepositoryBase<IRoleDb>
    {
        public RoleRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public IRoleDb FindByName(string roleName)
        {
            return dbSet.FirstOrDefault(x => x.Name == roleName);
        }

        public async Task<IRoleDb> FindByNameAsync(string name)
        {
            return await dbSet.FirstOrDefaultAsync(u => u.Name.ToUpper() == name.ToUpper());
        }

        public Task<IRoleDb> FindByNameAsync(System.Threading.CancellationToken cancellationToken, string roleName)
        {
            return dbSet.FirstOrDefaultAsync(x => x.Name.ToUpper() == roleName.ToUpper(), cancellationToken);
        }

        public async Task<List<string>> GetRolesNameByUserId(int userId)
        {
            var userRoles = db.Set<IUserRoleDb>();
            return await  (from role in dbSet
                           join user in userRoles on role.RoleId equals user.RoleId
                           where user.UserId == userId
                           select role.Name).ToListAsync();

        }

    }

}
