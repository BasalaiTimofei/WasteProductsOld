using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private readonly bool _initiateWithCS;

        public string NameOrConnectionString { get; }

        public UserRepository()
        {
            _initiateWithCS = false;
        }

        public UserRepository(string nameOrConnectionString)
        {
            _initiateWithCS = true;
            NameOrConnectionString = nameOrConnectionString;
        }

        /// <summary>
        /// Use ONLY with testDB!
        /// </summary>
        public void RecreateTestDatabase()
        {
            using (var db = GetWasteContext())
            {
                db.Database.Delete();
                db.Database.CreateIfNotExists();
            }
        }

        public async Task AddAsync(UserDB user, string password)
        {
            using(var userManager = GetUserManager())
            {
                user.Created = DateTime.UtcNow;
                await userManager.CreateAsync(user, password);
            }
        }

        public async Task AddClaimAsync(UserDB user, Claim claim)
        {
            using (var db = GetWasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.AddClaimAsync(user, claim);

                    user.Modified = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task AddLoginAsync(UserDB user, UserLoginDB login)
        {
            UserLoginInfo loginInfo = new UserLoginInfo(login.LoginProvider, login.ProviderKey);

            using (var db = GetWasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.AddLoginAsync(user, loginInfo);

                    user.Modified = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task AddToRoleAsync(UserDB user, string roleName)
        {
            using (var db = GetWasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.AddToRoleAsync(user, roleName);

                    user.Modified = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task AddFriendAsync(string userId, string friendId)
        {
            using (var db = GetWasteContext())
            {
                UserDB user = db.Users.Include(p => p.Friends).First(u => u.Id == userId);
                UserDB friend = db.Users.First(u => u.Id == friendId);

                user.Friends.Add(friend);

                user.Modified = DateTime.UtcNow;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(UserDB user)
        {
            await Task.Run(async () =>
            {
                using (var db = GetWasteContext())
                {
                    db.Users.Attach(user);
                    var entry = db.Entry(user);
                    entry.State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                }
            });
        }

        public async Task RemoveClaimAsync(UserDB user, Claim claim)
        {
            using (var db = GetWasteContext())
            {
                var userInDB = db.Users.Include(u => u.Claims).FirstOrDefault(u => u.Email == user.Email);

                var claimToDelete = userInDB.Claims.FirstOrDefault(c => c.UserId == userInDB.Id &&
                                                                        c.ClaimType == claim.Type &&
                                                                        c.ClaimValue == claim.Value);

                if (claimToDelete != null)
                {
                    db.Entry(claimToDelete).State = EntityState.Deleted;
                    user.Modified = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveFromRoleAsync(UserDB user, string roleName)
        {
            using (var db = GetWasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    await userStore.RemoveFromRoleAsync(user, roleName);
                    user.Modified = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveLoginAsync(UserDB user, UserLoginDB login)
        {
            using (var db = GetWasteContext())
            {
                var userInDB = db.Users.Include(u => u.Logins).FirstOrDefault(u => u.Email == user.Email);

                var loginToDelete = userInDB.Logins.FirstOrDefault(c => c.UserId == userInDB.Id &&
                                                                        c.LoginProvider == login.LoginProvider &&
                                                                        c.ProviderKey == login.ProviderKey);

                if (loginToDelete != null)
                {
                    db.Entry(loginToDelete).State = EntityState.Deleted;
                    user.Modified = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteFriendAsync(string userId, string deletingFriendId)
        {
            using (var db = GetWasteContext())
            {
                UserDB user = db.Users.Include(p => p.Friends).First(u => u.Id == userId);
                UserDB friend = db.Users.First(u => u.Id == deletingFriendId);

                user.Friends.Remove(friend);

                user.Modified = DateTime.UtcNow;
                await db.SaveChangesAsync();
            }
        }

        public UserDB Select(string email, string password, bool lazyInitiation = true)
        {
            return (Select(user => user.Email == email && user.PasswordHash == password, lazyInitiation, getRoles: false)).user;
        }

        public UserDB Select(string email, bool lazyInitiation = true)
        {
            return (Select(user => user.Email == email, lazyInitiation, getRoles: false)).user;
        }

        public UserDB Select(Func<UserDB, bool> predicate, bool lazyInitiation = true)
        {
            return (Select(predicate, lazyInitiation, getRoles: false)).user;
        }

        public (UserDB, IList<string>) Select(string email, string password)
        {
            WasteContext db = null;
            UserStore<UserDB> store = null;
            UserManager<UserDB> manager = null;
            try
            {
                db = GetWasteContext();
                db.Configuration.LazyLoadingEnabled = false;

                var user = db.Users.Include(u => u.Roles).
                        Include(u => u.Claims).
                        Include(u => u.Logins).
                        Include(u => u.Friends).
                        Include(u => u.Products).
                        FirstOrDefault(u => u.Email == email);

                store = new UserStore<UserDB>(db);
                manager = new UserManager<UserDB>(store);
                if (user != null && manager.CheckPassword(user, password))
                {
                    var roles = manager.GetRoles(user.Id);
                    return (user, roles);
                }
                else
                {
                    return (null, null);
                }
            }
            finally
            {
                db.Dispose();
                store.Dispose();
                manager.Dispose();
            }
        }

        public (UserDB user, IList<string> roles) SelectWithRoles(Func<UserDB, bool> predicate, bool lazyInitiation = true)
        {
            return Select(predicate, lazyInitiation, getRoles: true);
        }

        public List<UserDB> SelectAll()
        {
            using (var db = GetWasteContext())
            {
                return db.Users.ToList();
            }
        }

        public IEnumerable<UserDB> SelectRange(Func<UserDB, bool> predicate, bool lazyInitiation = true)
        {
            using (var db = GetWasteContext())
            {
                if (lazyInitiation)
                {
                    return db.Users.Where(predicate);
                }
                else
                {
                    db.Configuration.LazyLoadingEnabled = false;

                    // TODO expand with publicGroups when it awailable
                    return db.Users.Include(u => u.Roles).
                        Include(u => u.Claims).
                        Include(u => u.Logins).
                        Include(u => u.Friends).
                        Include(u => u.Products).
                        Where(predicate);
                }
            }
        }

        public async Task<IList<string>> GetRolesAsync(UserDB user)
        {
            using (var db = GetWasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    return await userStore.GetRolesAsync(user);
                }
            }
        }

        public async Task UpdateAsync(UserDB user)
        {
            using (var db = GetWasteContext())
            {
                db.Users.Attach(user);
                var entry = db.Entry(user);
                entry.State = EntityState.Modified;
                entry.Property(u => u.Created).IsModified = false;
                entry.Property(u => u.PasswordHash).IsModified = false;

                user.Modified = DateTime.UtcNow;
                await db.SaveChangesAsync();
            }
        }

        public async Task ResetPasswordAsync(UserDB user, string newPassword, string oldPassword)
        {
            using (var manager = GetUserManager())
            {
                await manager.ChangePasswordAsync(user.Id, oldPassword, newPassword);
            }
        }

        private (UserDB user, IList<string> roles) Select(Func<UserDB, bool> predicate, bool lazyInitiation, bool getRoles)
        {
            using (var db = GetWasteContext())
            {
                (UserDB user, IList<string> roles) result = (null, null);
                if (lazyInitiation)
                {
                    result.user = db.Users.FirstOrDefault(predicate);
                }
                else
                {
                    // TODO expand with groups when it will be available
                    db.Configuration.LazyLoadingEnabled = lazyInitiation;

                    result.user = db.Users.Include(u => u.Roles).
                        Include(u => u.Claims).
                        Include(u => u.Logins).
                        Include(u => u.Friends).
                        Include(u => u.Products).
                        FirstOrDefault(predicate);
                }

                if (getRoles)
                {
                    using (var userStore = new UserStore<UserDB>(db))
                    {
                        result.roles = userStore.GetRolesAsync(result.user).GetAwaiter().GetResult();
                    }
                }

                return result;
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

        private UserManager<UserDB> GetUserManager()
        {
            var db = GetWasteContext();
            var store = new UserStore<UserDB>(db)
            {
                DisposeContext = true
            };
            return new UserManager<UserDB>(store);
        }
    }
}
