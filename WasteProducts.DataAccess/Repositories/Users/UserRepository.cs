using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.Users;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly WasteContext _context;

        private readonly UserStore<UserDB> _store;

        private readonly UserManager<UserDB> _manager;

        private readonly IMapper _mapper;

        private bool _disposed;

        public UserRepository(WasteContext context, IMapper mapper)
        {
            _context = context;
            _store = new UserStore<UserDB>(_context)
            {
                DisposeContext = true
            };
            _manager = new UserManager<UserDB>(_store);

            _mapper = mapper;

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

        public async Task<(string id, string token)> AddAsync(string email, string userName, string password)
        {
            string id = Guid.NewGuid().ToString();

            var user = new UserDB
            {
                Id = id,
                Email = email,
                UserName = userName,
                Created = DateTime.UtcNow
            };
            await _manager.CreateAsync(user, password);
            if (await _manager.FindByIdAsync(id) != null)
            {
                _manager.UserTokenProvider = new EmailTokenProvider<UserDB>();
                var token = await _manager.GenerateEmailConfirmationTokenAsync(id);
                return (id, token);
            }
            else
            {
                return (null, null);
            }
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            _manager.UserTokenProvider = new EmailTokenProvider<UserDB>();
            await _manager.ConfirmEmailAsync(userId, token);
            if (await _manager.IsEmailConfirmedAsync(userId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<(string id, string token)> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email);
            _manager.UserTokenProvider = new TotpSecurityStampBasedTokenProvider<UserDB, string>();
            var token = await _manager.GeneratePasswordResetTokenAsync(user.Id);
            return (user.Id, token);
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var result = await _manager.ResetPasswordAsync(userId, token, newPassword);
            return result.Succeeded;
        }

        public async Task<UserDAL> GetByNameAndPasswordAsync(string userName, string password)
        {
            return MapTo<UserDAL>(await _manager.FindAsync(userName, password));
        }

        public async Task<UserDAL> GetByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _manager.FindByEmailAsync(email);
            if (user != null && await _manager.CheckPasswordAsync(user, password))
            {
                return MapTo<UserDAL>(user);
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

        public async Task<IEnumerable<UserDAL>> GetAllAsync(bool initiateNavigationalProps)
        {
            return await Task.Run(() =>
            {
                if (initiateNavigationalProps)
                {
                    return _mapper.Map<IEnumerable<UserDAL>>(_context.Users.ToList());
                }
                else
                {
                    var subresult = _context.Users.Include(u => u.Roles).
                        Include(u => u.Claims).
                        Include(u => u.Logins).
                        Include(u => u.Friends).
                        Include(u => u.ProductDescriptions.Select(p => p.Product)).ToList();

                    return _mapper.Map<IEnumerable<UserDAL>>(subresult);
                }
            });
        }

        public async Task<UserDAL> GetAsync(string id, bool initiateNavigationalProps)
        {
            return await Task.Run(() =>
            {
                if (initiateNavigationalProps)
                {
                    var subresult = _context.Users.FirstOrDefault(u => u.Id == id);
                    return MapTo<UserDAL>(subresult);
                }
                else
                {
                    var subresult = _context.Users.Include(u => u.Roles).
                        Include(u => u.Claims).
                        Include(u => u.Logins).
                        Include(u => u.Friends).
                        Include(u => u.ProductDescriptions.Select(p => p.Product))
                        .FirstOrDefault(u => u.Id == id);

                    return MapTo<UserDAL>(subresult);
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

        public async Task UpdateAsync(UserDAL user)
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
            await Task.Run(() =>
            {
                UserDB user = _context.Users.Include(p => p.Friends).FirstOrDefault(u => u.Id == userId);
                UserDB friend = _context.Users.FirstOrDefault(u => u.Id == friendId);

                if (user != null && friend != null)
                {
                    user.Friends.Add(friend);
                    user.Modified = DateTime.UtcNow;
                    _context.SaveChanges();
                }
            });
        }

        public async Task<IList<UserDAL>> GetFriendsAsync(string userId)
        {
            return await Task.Run(() =>
            {
                var user = _context.Users.Include(u => u.Friends).FirstOrDefault(u => u.Id == userId);
                return _mapper.Map<List<UserDAL>>(user.Friends);
            });
        }

        public async Task DeleteFriendAsync(string userId, string deletingFriendId)
        {
            await Task.Run(() =>
            {
                UserDB user = _context.Users.Include(p => p.Friends).FirstOrDefault(u => u.Id == userId);
                UserDB friend = _context.Users.FirstOrDefault(u => u.Id == deletingFriendId);

                if (user != null && friend != null)
                {
                    user.Friends.Remove(friend);
                    user.Modified = DateTime.UtcNow;
                    _context.SaveChanges();
                }
            });
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

        public async Task<IList<UserProductDescriptionDB>> GetProductDescriptionsAsync(string userId)
        {
            return await Task.Run(() =>
            {
                var user = _context.Users.Include(u => u.ProductDescriptions).FirstOrDefault(u => u.Id == userId);
                return user.ProductDescriptions;
            });
        }

        public async Task<bool> UpdateProductDescriptionAsync(string userId, string productId, int rating, string description)
        {
            return await Task.Run(() =>
            {
                UserProductDescriptionDB descr = _context.UserProductDescriptions.FirstOrDefault(d => d.UserId == userId && d.ProductId == productId);
                if (descr == null)
                {
                    return false;
                }
                descr.Rating = rating;
                descr.Description = description;

                _context.SaveChanges();
                return true;
            });
        }

        public async Task<bool> DeleteProductAsync(string userId, string productId)
        {
            return await Task.Run(() =>
            {
                var description = _context.UserProductDescriptions.FirstOrDefault(d => d.User.Id == userId && d.Product.Id == productId);
                if (description != null)
                {
                    var entry = _context.Entry(description);
                    entry.State = EntityState.Deleted;
                    _context.SaveChanges();
                    return true;
                }
                else return false;
            });
        }

        public async Task RespondToGroupInvitation(string userId, string groupId, bool isConfirmed)
        {
            await Task.Run(() =>
            {
                var groupUser = _context.GroupUsers.FirstOrDefault(u => u.UserId == userId && u.GroupId == groupId);
                if (groupUser != null)
                {
                    var entry = _context.Entry(groupUser);
                    if (isConfirmed)
                    {
                        groupUser.IsConfirmed = true;
                        groupUser.Modified = DateTime.UtcNow;
                        entry.State = EntityState.Unchanged;
                        entry.Property(p => p.Modified).IsModified = true;
                        entry.Property(p => p.IsConfirmed).IsModified = true;
                    }
                    else
                    {
                        entry.State = EntityState.Deleted;
                    }
                    _context.SaveChanges();
                }
            });
        }

        public async Task<IEnumerable<GroupUserDB>> GetGroups(string userId)
        {
            return await Task.Run(() =>
            {
                var user = _context.Users.Include(u => u.Groups.Select(g => g.Group)).FirstOrDefault(u => u.Id == userId);
                return user.Groups.Where(g => g.Group.IsNotDeleted);
            });
        }

        private UserDB MapTo<T>(UserDAL user)
            where T : UserDB
            =>
            _mapper.Map<UserDB>(user);

        private UserDAL MapTo<T>(UserDB user)
            where T : UserDAL
            =>
            _mapper.Map<UserDAL>(user);

        ~UserRepository()
        {
            Dispose();
        }
    }
}
