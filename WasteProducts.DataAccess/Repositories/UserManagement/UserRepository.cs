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

        public async Task AddAsync(UserDB user)
        {
            using (var db = GetWasteContext())
            {
                using (var userStore = new UserStore<UserDB>(db))
                {
                    user.Created = DateTime.UtcNow;
                    await userStore.CreateAsync(user);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task AddClaimAsync(UserDB user, Claim claim)
        {
            using (var db = GetWasteContext())
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
            using (var db = GetWasteContext())
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
            using (var db = GetWasteContext())
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
            using (var db = GetWasteContext())
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
            using (var db = GetWasteContext())
            {
                var userInDB = db.Users.Include(u => u.Claims).Where(u => u.Email == user.Email).FirstOrDefault();

                var claimToDelete = userInDB.Claims.Where(c => c.UserId == userInDB.Id &&
                                                               c.ClaimType == claim.Type &&
                                                               c.ClaimValue == claim.Value)
                                                               .FirstOrDefault();

                if (claimToDelete != null)
                {
                    db.Entry(claimToDelete).State = EntityState.Deleted;
                }

                await db.SaveChangesAsync();
            }
        }

        public async Task RemoveFromRoleAsync(UserDB user, string roleName)
        {
            using (var db = GetWasteContext())
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
            using (var db = GetWasteContext())
            {
                var userInDB = db.Users.Include(u => u.Logins).Where(u => u.Email == user.Email).FirstOrDefault();

                var loginToDelete = userInDB.Logins.Where(c => c.UserId == userInDB.Id &&
                                                               c.LoginProvider == login.LoginProvider &&
                                                               c.ProviderKey == login.ProviderKey)
                                                               .FirstOrDefault();

                if (loginToDelete != null)
                {
                    db.Entry(loginToDelete).State = EntityState.Deleted;
                }

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

        public List<UserDB> SelectRange(Func<UserDB, bool> predicate)
        {
            using (var db = GetWasteContext())
            {
                return db.Users.Where(predicate).ToList();
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

        private (UserDB user, IList<string> roles) Select(Func<UserDB, bool> predicate, bool lazyInitiation, bool getRoles)
        {
            using (var db = GetWasteContext())
            {
                (UserDB user, IList<string> roles) result = (null, null);
                if (lazyInitiation)
                {
                    result.user = db.Users.Where(predicate).FirstOrDefault();
                }
                else
                {
                    // TODO expand with groups when it available
                    db.Configuration.LazyLoadingEnabled = lazyInitiation;

                    result.user = db.Users.Include(u => u.Roles).
                        Include(u => u.Claims).
                        Include(u => u.Logins).
                        Include(u => u.Friends).
                        Include(u => u.Products).
                        Where(predicate).FirstOrDefault();
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
    }
}
