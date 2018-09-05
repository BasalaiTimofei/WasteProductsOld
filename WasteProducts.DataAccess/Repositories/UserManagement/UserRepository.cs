using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.UserManagement
{
    public class UserRepository : IUserRepository
    {
        private readonly WasteContext _db;

        private readonly UserStore<UserDB> _store;

        private readonly UserManager<UserDB> _manager;

        private bool _disposed;

        public UserRepository()
        {
            _db = new WasteContext();
            _store = new UserStore<UserDB>(_db)
            {
                DisposeContext = true
            };
            _manager = new UserManager<UserDB>(_store);
        }

        public UserRepository(string nameOrConnectionString)
        {
            _db = new WasteContext(nameOrConnectionString);
            _store = new UserStore<UserDB>(_db)
            {
                DisposeContext = true
            };
            _manager = new UserManager<UserDB>(_store);
        }

        ~UserRepository()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _manager?.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Use ONLY with TestDB!
        /// </summary>
        public void RecreateTestDatabase()
        {
                _db.Database.Delete();
                _db.Database.CreateIfNotExists();
        }

        public async Task AddAsync(UserDB user, string password)
        {
                user.Created = DateTime.UtcNow;
                await _manager.CreateAsync(user, password);
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return !(await _db.Users.AnyAsync(u => u.Email == email));
        }

        public async Task AddClaimAsync(UserDB user, Claim claim)
        {
            await _store.AddClaimAsync(user, claim);

            user.Modified = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task AddLoginAsync(UserDB user, UserLoginDB login)
        {
            UserLoginInfo loginInfo = new UserLoginInfo(login.LoginProvider, login.ProviderKey);

            using (var userStore = new UserStore<UserDB>(_db))
            {
                await userStore.AddLoginAsync(user, loginInfo);

                user.Modified = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        public async Task AddToRoleAsync(UserDB user, string roleName)
        {
            using (var userStore = new UserStore<UserDB>(_db))
            {
                await userStore.AddToRoleAsync(user, roleName);

                user.Modified = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        public async Task AddFriendAsync(string userId, string friendId)
        {
            UserDB user = _db.Users.Include(p => p.Friends).First(u => u.Id == userId);
            UserDB friend = _db.Users.First(u => u.Id == friendId);

            user.Friends.Add(friend);

            user.Modified = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteFriendAsync(string userId, string deletingFriendId)
        {
            UserDB user = _db.Users.Include(p => p.Friends).First(u => u.Id == userId);
            UserDB friend = _db.Users.First(u => u.Id == deletingFriendId);

            user.Friends.Remove(friend);

            user.Modified = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task<bool> AddProductAsync(string userId, string productId, int rating, string description)
        {
            return await Task.Run(() =>
            {
                _db.Configuration.LazyLoadingEnabled = false;
                UserDB user = null;
                ProductDB product = null;
                try
                {
                    user = _db.Users.Include(u => u.ProductDescriptions).First(u => u.Id == userId);
                    product = _db.Products.FirstOrDefault(p => p.Id == productId);
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                var userProdDescr = new UserProductDescriptionDB()
                {
                    User = user,
                    Product = product,
                    Rating = rating,
                    Description = description,
                    Created = DateTime.UtcNow
                };
                _db.UserProductDescriptions.Add(userProdDescr);
                _db.SaveChanges();
                return true;
            });
        }

        public async Task<bool> DeleteProductAsync(string userId, string productId)
        {
            return await Task.Run(() =>
            {
                UserProductDescriptionDB description = null;
                try
                {
                    description = _db.UserProductDescriptions.First(d => d.User.Id == userId && d.Product.Id == productId);
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                var entry = _db.Entry(description);
                entry.State = EntityState.Deleted;
                _db.SaveChanges();
                return true;
            });
        }

        public async Task DeleteAsync(string userId)
        {
            await Task.Run(async () =>
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                var entry = _db.Entry(user);
                entry.State = EntityState.Deleted;
                await _db.SaveChangesAsync();
            });
        }

        public async Task RemoveClaimAsync(UserDB user, Claim claim)
        {
            var userInDB = _db.Users.Include(u => u.Claims).FirstOrDefault(u => u.Email == user.Email);

            var claimToDelete = userInDB.Claims.FirstOrDefault(c => c.UserId == userInDB.Id &&
                                                                    c.ClaimType == claim.Type &&
                                                                    c.ClaimValue == claim.Value);

            if (claimToDelete != null)
            {
                _db.Entry(claimToDelete).State = EntityState.Deleted;
                user.Modified = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveFromRoleAsync(UserDB user, string roleName)
        {
            using (var userStore = new UserStore<UserDB>(_db))
            {
                await userStore.RemoveFromRoleAsync(user, roleName);
                user.Modified = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveLoginAsync(UserDB user, UserLoginDB login)
        {
            var userInDB = _db.Users.Include(u => u.Logins).FirstOrDefault(u => u.Email == user.Email);

            var loginToDelete = userInDB.Logins.FirstOrDefault(c => c.UserId == userInDB.Id &&
                                                                    c.LoginProvider == login.LoginProvider &&
                                                                    c.ProviderKey == login.ProviderKey);

            if (loginToDelete != null)
            {
                _db.Entry(loginToDelete).State = EntityState.Deleted;
                user.Modified = DateTime.UtcNow;
                await _db.SaveChangesAsync();
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
            _db.Configuration.LazyLoadingEnabled = false;

            var user = _db.Users.Include(u => u.Roles).
                    Include(u => u.Claims).
                    Include(u => u.Logins).
                    Include(u => u.Friends).
                    Include(u => u.ProductDescriptions).
                    FirstOrDefault(u => u.Email == email);

            if (user != null && _manager.CheckPassword(user, password))
            {
                var roles = _manager.GetRoles(user.Id);
                return (user, roles);
            }
            else
            {
                return (null, null);
            }
        }

        public (UserDB user, IList<string> roles) SelectWithRoles(Func<UserDB, bool> predicate, bool lazyInitiation = true)
        {
            return Select(predicate, lazyInitiation, getRoles: true);
        }

        //todo fix method
        public List<UserDB> SelectAll()
        {
            return _db.Users.ToList();
        }

        public IEnumerable<UserDB> SelectRange(Func<UserDB, bool> predicate, bool lazyInitiation = true)
        {
            if (lazyInitiation)
            {
                return _db.Users.Where(predicate);
            }
            else
            {
                _db.Configuration.LazyLoadingEnabled = false;

                // TODO expand with publicGroups when it awailable
                return _db.Users.Include(u => u.Roles).
                    Include(u => u.Claims).
                    Include(u => u.Logins).
                    Include(u => u.Friends).
                    Include(u => u.ProductDescriptions).
                    Where(predicate);
            }
        }

        public async Task<IList<string>> GetRolesAsync(UserDB user)
        {
            return await _store.GetRolesAsync(user);
        }

        public async Task ResetPasswordAsync(UserDB user, string newPassword, string oldPassword)
        {
            await _manager.ChangePasswordAsync(user.Id, oldPassword, newPassword);
        }

        public async Task UpdateAsync(UserDB user)
        {
            var userInDB = _db.Users.FirstOrDefault(u => u.Id == user.Id);

            var entry = _db.Entry(userInDB);
            entry.CurrentValues.SetValues(user);
            entry.Property(u => u.Modified).CurrentValue = DateTime.UtcNow;

            entry.Property(u => u.UserName).IsModified = false;
            entry.Property(u => u.Email).IsModified = false;
            entry.Property(u => u.Created).IsModified = false;
            entry.Property(u => u.PasswordHash).IsModified = false;

            await _db.SaveChangesAsync();
        }

        public async Task<bool> UpdateEmailAsync(string userId, string newEmail)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null && await IsEmailAvailableAsync(newEmail))
            {
                user.Modified = DateTime.UtcNow;
                user.Email = newEmail;
                var entry = _db.Entry(user);
                entry.Property(u => u.Email).IsModified = true;
                entry.Property(u => u.Modified).IsModified = true;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateUserNameAsync(UserDB user, string newUserName)
        {
            bool userNameAvailable = !(await _db.Users.AnyAsync(u => u.UserName == newUserName));
            if (userNameAvailable)
            {
                user.Modified = DateTime.UtcNow;
                _db.Users.Attach(user);
                var entry = _db.Entry(user);
                entry.Property(u => u.UserName).IsModified = true;
                await _db.SaveChangesAsync();
            }
            return userNameAvailable;
        }

        private (UserDB user, IList<string> roles) Select(Func<UserDB, bool> predicate, bool lazyInitiation, bool getRoles)
        {
            (UserDB user, IList<string> roles) result = (null, null);
            if (lazyInitiation)
            {
                result.user = _db.Users.FirstOrDefault(predicate);
            }
            else
            {
                _db.Configuration.LazyLoadingEnabled = lazyInitiation;

                result.user = _db.Users.Include(u => u.Roles).
                    Include(u => u.Claims).
                    Include(u => u.Logins).
                    Include(u => u.Friends).
                    Include(u => u.ProductDescriptions.Select(p => p.Product)).
                    FirstOrDefault(predicate);
            }

            if (getRoles)
            {
                using (var userStore = new UserStore<UserDB>(_db))
                {
                    result.roles = userStore.GetRolesAsync(result.user).GetAwaiter().GetResult();
                }
            }

            return result;
        }
    }
}
