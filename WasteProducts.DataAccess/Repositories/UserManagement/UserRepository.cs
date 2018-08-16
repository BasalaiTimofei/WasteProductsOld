using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.UserManagement
{
    public class UserRepository : IUserRepository
    {
        public async Task AddAsync(UserDB user)
        {
            using (var db = new WasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.CreateAsync(user);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task AddClaimAsync(UserDB user, Claim claim)
        {
            using (var db = new WasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.AddClaimAsync(user, claim);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task AddLoginAsync(UserDB user, UserLoginInfo login)
        {
            using (var db = new WasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.AddLoginAsync(user, login);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task AddToRoleAsync(UserDB user, string roleName)
        {
            using (var db = new WasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.AddToRoleAsync(user, roleName);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAsync(UserDB user)
        {
            using (var db = new WasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.DeleteAsync(user);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveClaimAsync(UserDB user, Claim claim)
        {
            using (var db = new WasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.RemoveClaimAsync(user, claim);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveFromRoleAsync(UserDB user, string roleName)
        {
            using (var db = new WasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.RemoveFromRoleAsync(user, roleName);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveLoginAsync(UserDB user, UserLoginInfo login)
        {
            using (var db = new WasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.RemoveLoginAsync(user, login);
                    await db.SaveChangesAsync();
                }
            }
        }

        public UserDB Select(string email, string password)
        {
            return Select(user => user.Email == email && user.PasswordHash == password);
        }

        public UserDB Select(string email)
        {
            return Select(user => user.Email == email);
        }

        public UserDB Select(Func<UserDB, bool> predicate)
        {
            using (var db = new WasteContext())
            {
                return db.Users.Where(predicate).FirstOrDefault();
            }
        }

        public List<UserDB> SelectAll()
        {
            using (var db = new WasteContext())
            {
                return db.Users.ToList();
            }
        }

        public List<UserDB> SelectRange(Func<UserDB, bool> predicate)
        {
            using (var db = new WasteContext())
            {
                return db.Users.Where(predicate).ToList();
            }
        }

        public async Task UpdateAsync(UserDB user)
        {
            user.Modified = DateTime.UtcNow;

            using (var db = new WasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.UpdateAsync(user);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
