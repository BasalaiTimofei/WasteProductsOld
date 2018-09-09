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
        private readonly WasteContext _context;

        private readonly UserStore<UserDB> _store;

        private readonly UserManager<UserDB> _manager;

        private bool _disposed;

        public UserRepository()
        {
            _context = new WasteContext();
            _store = new UserStore<UserDB>(_context)
            {
                DisposeContext = true
            };
            _manager = new UserManager<UserDB>(_store);
        }

        public UserRepository(WasteContext context)
        {
            _context = context;
            _store = new UserStore<UserDB>(_context)
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
                _context.Database.Delete();
                _context.Database.CreateIfNotExists();
        }

        public async Task AddAsync(UserDB user, string password)
        {
            user.Created = DateTime.UtcNow;
            await _manager.CreateAsync(user, password);
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return !(await _context.Users.AnyAsync(u => u.Email == email));
        }

        public async Task AddClaimAsync(string userId, Claim claim)
        {
            await _manager.AddClaimAsync(userId, claim);
        }

        public async Task AddLoginAsync(string userId, UserLoginDB login)
        {
            var userLoginInfo = new UserLoginInfo(login.LoginProvider, login.ProviderKey);
            await _manager.AddLoginAsync(userId, userLoginInfo);
        }

        public async Task AddToRoleAsync(string userId, string roleName)
        {
            await _manager.AddToRoleAsync(userId, roleName);
        }

        public async Task AddFriendAsync(string userId, string friendId)
        {
            UserDB user = _context.Users.Include(p => p.Friends).FirstOrDefault(u => u.Id == userId);
            UserDB friend = _context.Users.FirstOrDefault(u => u.Id == friendId);

            if(user != null && friend != null)
            {
                user.Friends.Add(friend);
                user.Modified = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFriendAsync(string userId, string deletingFriendId)
        {
            UserDB user = _context.Users.Include(p => p.Friends).FirstOrDefault(u => u.Id == userId);
            UserDB friend = _context.Users.FirstOrDefault(u => u.Id == deletingFriendId);

            if (user != null && friend != null)
            {
                user.Friends.Remove(friend);
                user.Modified = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AddProductAsync(string userId, string productId, int rating, string description)
        {
            return await Task.Run(() =>
            {
                UserDB user = null;
                ProductDB product = null;
                try
                {
                    user = _context.Users.Include(u => u.ProductDescriptions).First(u => u.Id == userId);
                    product = _context.Products.FirstOrDefault(p => p.Id == productId);
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
                _context.UserProductDescriptions.Add(userProdDescr);
                _context.SaveChanges();
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
                    description = _context.UserProductDescriptions.First(d => d.User.Id == userId && d.Product.Id == productId);
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                var entry = _context.Entry(description);
                entry.State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            });
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await _manager.FindByIdAsync(userId);
            await _manager.DeleteAsync(user);
        }

        public async Task RemoveClaimAsync(string userId, Claim claim)
        {
            await _manager.RemoveClaimAsync(userId, claim);
        }

        public async Task RemoveFromRoleAsync(string userId, string roleName)
        {
            await _manager.RemoveFromRoleAsync(userId, roleName);
        }

        public async Task RemoveLoginAsync(string userId, UserLoginDB login)
        {
            var userLoginInfo = new UserLoginInfo(login.LoginProvider, login.ProviderKey);
            await _manager.RemoveLoginAsync(userId, userLoginInfo);
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
            _context.Configuration.LazyLoadingEnabled = false;

            var user = _context.Users.Include(u => u.Roles).
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
            return _context.Users.ToList();
        }

        public IEnumerable<UserDB> SelectRange(Func<UserDB, bool> predicate, bool lazyInitiation = true)
        {
            if (lazyInitiation)
            {
                return _context.Users.Where(predicate);
            }
            else
            {
                _context.Configuration.LazyLoadingEnabled = false;

                // TODO expand with publicGroups when it awailable
                return _context.Users.Include(u => u.Roles).
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
            var userInDB = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            var entry = _context.Entry(userInDB);
            entry.CurrentValues.SetValues(user);
            entry.Property(u => u.Modified).CurrentValue = DateTime.UtcNow;

            entry.Property(u => u.UserName).IsModified = false;
            entry.Property(u => u.Email).IsModified = false;
            entry.Property(u => u.Created).IsModified = false;
            entry.Property(u => u.PasswordHash).IsModified = false;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateEmailAsync(string userId, string newEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null && await IsEmailAvailableAsync(newEmail))
            {
                user.Modified = DateTime.UtcNow;
                user.Email = newEmail;
                var entry = _context.Entry(user);
                entry.Property(u => u.Email).IsModified = true;
                entry.Property(u => u.Modified).IsModified = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateUserNameAsync(UserDB user, string newUserName)
        {
            bool userNameAvailable = !(await _context.Users.AnyAsync(u => u.UserName == newUserName));
            if (userNameAvailable)
            {
                user.Modified = DateTime.UtcNow;
                _context.Users.Attach(user);
                var entry = _context.Entry(user);
                entry.Property(u => u.UserName).IsModified = true;
                await _context.SaveChangesAsync();
            }
            return userNameAvailable;
        }

        private (UserDB user, IList<string> roles) Select(Func<UserDB, bool> predicate, bool lazyInitiation, bool getRoles)
        {
            (UserDB user, IList<string> roles) result = (null, null);
            if (lazyInitiation)
            {
                result.user = _context.Users.FirstOrDefault(predicate);
            }
            else
            {
                _context.Configuration.LazyLoadingEnabled = lazyInitiation;

                result.user = _context.Users.Include(u => u.Roles).
                    Include(u => u.Claims).
                    Include(u => u.Logins).
                    Include(u => u.Friends).
                    Include(u => u.ProductDescriptions.Select(p => p.Product)).
                    FirstOrDefault(predicate);
            }

            if (getRoles)
            {
                using (var userStore = new UserStore<UserDB>(_context))
                {
                    result.roles = userStore.GetRolesAsync(result.user).GetAwaiter().GetResult();
                }
            }

            return result;
        }
    }
}
