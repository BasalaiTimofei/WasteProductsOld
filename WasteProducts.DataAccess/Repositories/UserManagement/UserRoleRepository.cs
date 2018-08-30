using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;
using System.Data.Entity;

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
            using (var db = GetWasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    IdentityRole identityRole = await roleStore.FindByIdAsync(role.Id);
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
                    if (ir == null)
                    {
                        return null;
                    }
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
            UserRepository userRepo = new UserRepository();

            using (var db = GetWasteContext())
            {
                using (var roleStore = new RoleStore<IdentityRole>(db))
                {
                    IdentityRole ir = await roleStore.FindByIdAsync(role.Id);
                    List<string> userIds = new List<string>();

                    foreach (IdentityUserRole iur in ir.Users)
                    {
                        userIds.Add(iur.UserId);
                    }

                    IEnumerable<UserDB> result = db.Users.Include(u => u.Roles).
                                                          Include(u => u.Claims).
                                                          Include(u => u.Logins).
                                                          Include(u => u.Friends).
                                                          Include(u => u.Products).
                                                          Where(u => userIds.Contains(u.Id));

                    return result.ToArray();
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
