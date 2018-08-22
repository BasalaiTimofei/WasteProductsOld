using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.UserManagement
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly bool _initiateWithCS;

        public string NameOrConnectionString { get; }

        public UserRoleRepository()
        {
            _initiateWithCS = false;
        }

        public UserRoleRepository(string nameOrConnectionString)
        {
            _initiateWithCS = true;
            NameOrConnectionString = nameOrConnectionString;
        }

        public async Task AddAsync(UserRoleDB role)
        {
            IdentityRole identityRole = new IdentityRole(role.Name) { Id = Guid.NewGuid().ToString() };

            using (var db = GetWasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    await roleStore.CreateAsync(identityRole);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAsync(UserRoleDB role)
        {
            IdentityRole identityRole = new IdentityRole(role.Name) { Id = role.Id };

            using (var db = GetWasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    await roleStore.DeleteAsync(identityRole);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<UserRoleDB> FindByIdAsync(string roleId)
        {
            using (var db = GetWasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    IdentityRole ir = await roleStore.FindByIdAsync(roleId);
                    UserRoleDB result = new UserRoleDB() { Id = ir.Id, Name = ir.Name };
                    return result;
                }
            }
        }

        public async Task<UserRoleDB> FindByNameAsync(string roleName)
        {
            using (var db = GetWasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    IdentityRole ir = await roleStore.FindByNameAsync(roleName);
                    UserRoleDB result = new UserRoleDB() { Id = ir.Id, Name = ir.Name };
                    return result;
                }
            }
        }

        public async Task UpdateRoleNameAsync(UserRoleDB role)
        {
            using (var db = GetWasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    IdentityRole r = await roleStore.FindByIdAsync(role.Id);
                    r.Name = role.Name;
                    await roleStore.UpdateAsync(r);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<UserDB>> GetRoleUsers(UserRoleDB role)
        {
            using (var db = GetWasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    IdentityRole ir = await roleStore.FindByIdAsync(role.Id);
                    return (IEnumerable<UserDB>)ir.Users;
                }
            }
        }

        private WasteContext GetWasteContext()
        {
            if (_initiateWithCS)
            {
                return new WasteContext(NameOrConnectionString);
            }
            else
            {
                return new WasteContext();
            }
        }
    }
}
