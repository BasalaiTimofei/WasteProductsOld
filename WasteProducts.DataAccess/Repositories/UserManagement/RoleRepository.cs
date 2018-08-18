using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.UserManagement
{
    public class UserRoleRepository : IUserRoleRepository
    {
        public async Task AddAsync(IdentityRole role)
        {
            using (var db = new WasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    await roleStore.CreateAsync(role);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAsync(IdentityRole role)
        {
            using (var db = new WasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    await roleStore.DeleteAsync(role);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId)
        {
            using (var db = new WasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    return await roleStore.FindByIdAsync(roleId);
                }
            }
        }

        public async Task<IdentityRole> FindByNameAsync(string roleName)
        {
            using (var db = new WasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    return await roleStore.FindByNameAsync(roleName);
                }
            }
        }

        public async Task UpdateAsync(IdentityRole role)
        {
            using (var db = new WasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    await roleStore.UpdateAsync(role);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
