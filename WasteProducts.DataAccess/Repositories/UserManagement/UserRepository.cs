using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.Search;
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

        public UserRepository(WasteContext context)
        {
            _context = context;
            _store = new UserStore<UserDB>(_context)
            {
                DisposeContext = true
            };
            _manager = new UserManager<UserDB>(_store);
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

        public async Task AddAsync(string email, string userName, string password)
        {
            var user = new UserDB
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                UserName = userName,
                Created = DateTime.UtcNow
            };
            await _manager.CreateAsync(user, password);
        }

        public async Task<UserDB> FindByNameAndPasswordAsync(string userName, string password)
        {
            return await _manager.FindAsync(userName, password);
        }

        public async Task<UserDB> FindByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _manager.FindByEmailAsync(email);
            if (user != null && await _manager.CheckPasswordAsync(user, password))
            {
                return user;
            }
            else
            {
                return null;
            }
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

        public IQueryable<UserDB> GetSelector(bool initiateNavigationalProps)
        {
            if (initiateNavigationalProps)
            {
                return _context.Users;
            }
            else
            {
                return _context.Users.Include(u => u.Roles).
                    Include(u => u.Claims).
                    Include(u => u.Logins).
                    Include(u => u.Friends).
                    Include(u => u.ProductDescriptions.Select(p => p.Product));
            }
        }

        public async Task<UserDB> GetAsync(Func<UserDB, bool> predicate, bool initiateNavigationalProps)
        {
            return await Task.Run(() =>
            {
                if (initiateNavigationalProps)
                {
                    return _context.Users.FirstOrDefault(predicate);
                }
                else
                {
                    return _context.Users.Include(u => u.Roles).
                        Include(u => u.Claims).
                        Include(u => u.Logins).
                        Include(u => u.Friends).
                        Include(u => u.ProductDescriptions.Select(p => p.Product))
                        .FirstOrDefault(predicate);
                }
            });
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            return await _manager.GetRolesAsync(userId);
        }

        public async Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            return await _manager.GetClaimsAsync(userId);
        }

        public async Task<IList<UserLoginDB>> GetLoginsAsync(string userId)
        {
            var logins = await _manager.GetLoginsAsync(userId);
            IList<UserLoginDB> result = new List<UserLoginDB>(logins.Count);

            foreach (var login in logins)
            {
                result.Add(new UserLoginDB { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
            }
            return result;
        }

        public async Task ChangePasswordAsync(string userId, string newPassword, string oldPassword)
        {
            await _manager.ChangePasswordAsync(userId, oldPassword, newPassword);
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

        public async Task<bool> UpdateUserNameAsync(string userId, string newUserName)
        {
            bool userNameAvailable = !(await _context.Users.AnyAsync(u => u.UserName == newUserName));

            if (userNameAvailable)
            {
                var user = _manager.FindById(userId);

                user.Modified = DateTime.UtcNow;
                user.UserName = newUserName;

                var entry = _context.Entry(user);

                entry.State = EntityState.Unchanged;
                entry.Property(u => u.UserName).IsModified = true;
                entry.Property(u => u.Modified).IsModified = true;

                await _context.SaveChangesAsync();
            }
            return userNameAvailable;
        }

        // Business logic below
        public async Task AddFriendAsync(string userId, string friendId)
        {
            UserDB user = _context.Users.Include(p => p.Friends).FirstOrDefault(u => u.Id == userId);
            UserDB friend = _context.Users.FirstOrDefault(u => u.Id == friendId);

            if (user != null && friend != null)
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

        public async Task<bool> AddProductAsync(string userId, string productId, int? rating, string description)
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

        public async Task<bool> UpdateProductDescriptionAsync(string userId, string productId, int? rating, string description)
        {
            return await Task.Run(async () =>
            {
                UserProductDescriptionDB descr = _context.UserProductDescriptions.FirstOrDefault(d => d.UserId == userId && d.ProductId == productId);
                if (descr == null)
                {
                    return false;
                }
                descr.Rating = (int)rating;
                descr.Description = description;

                await _context.SaveChangesAsync();
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

        ~UserRepository()
        {
            Dispose();
        }
    }
}
