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
            await _manager.CreateAsync(user, password).ConfigureAwait(false);
            if (await _manager.FindByIdAsync(id).ConfigureAwait(false) != null)
            {
                _manager.UserTokenProvider = new EmailTokenProvider<UserDB>();
                var token = await _manager.GenerateEmailConfirmationTokenAsync(id).ConfigureAwait(false);
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
            await _manager.ConfirmEmailAsync(userId, token).ConfigureAwait(false);
            if (await _manager.IsEmailConfirmedAsync(userId).ConfigureAwait(false))
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
            var user = await _manager.FindByEmailAsync(email).ConfigureAwait(false);
            _manager.UserTokenProvider = new TotpSecurityStampBasedTokenProvider<UserDB, string>();
            var token = await _manager.GeneratePasswordResetTokenAsync(user.Id).ConfigureAwait(false);
            return (user.Id, token);
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var result = await _manager.ResetPasswordAsync(userId, token, newPassword).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<UserDAL> GetByNameAndPasswordAsync(string userName, string password)
        {
            return MapTo<UserDAL>(await _manager.FindAsync(userName, password).ConfigureAwait(false));
        }

        public async Task<UserDAL> GetByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _manager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user != null && await _manager.CheckPasswordAsync(user, password).ConfigureAwait(false))
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
            return !(await _context.Users.AnyAsync(u => u.Email == email).ConfigureAwait(false));
        }

        public async Task AddClaimAsync(string userId, Claim claim)
        {
            await _manager.AddClaimAsync(userId, claim).ConfigureAwait(false);
        }

        public async Task AddLoginAsync(string userId, UserLoginDB login)
        {
            var userLoginInfo = new UserLoginInfo(login.LoginProvider, login.ProviderKey);
            await _manager.AddLoginAsync(userId, userLoginInfo).ConfigureAwait(false);
        }

        public async Task AddToRoleAsync(string userId, string roleName)
        {
            await _manager.AddToRoleAsync(userId, roleName).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await _context.Users.
                        Include(u => u.Friends).
                        Include(u => u.ProductDescriptions).
                        Include(u => u.Groups.Select(g => g.Group))
                        .FirstOrDefaultAsync(u => u.Id == userId)
                        .ConfigureAwait(false);

            user.Groups.Clear();
            user.ProductDescriptions.Clear();
            user.Friends.Clear();

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.AdminId == user.Id).ConfigureAwait(false);
            if (group != null)
            {
                _context.Groups.Remove(group);
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
            await _manager.DeleteAsync(user).ConfigureAwait(false);
        }

        public async Task RemoveClaimAsync(string userId, Claim claim)
        {
            await _manager.RemoveClaimAsync(userId, claim).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(string userId, string roleName)
        {
            await _manager.RemoveFromRoleAsync(userId, roleName).ConfigureAwait(false);
        }

        public async Task RemoveLoginAsync(string userId, UserLoginDB login)
        {
            var userLoginInfo = new UserLoginInfo(login.LoginProvider, login.ProviderKey);
            await _manager.RemoveLoginAsync(userId, userLoginInfo).ConfigureAwait(false);
        }

        public async Task<IEnumerable<UserDAL>> GetAllAsync()
        {
            var subresult = await _context.Users.ToListAsync().ConfigureAwait(false);
            return _mapper.Map<IEnumerable<UserDAL>>(subresult);
        }

        public async Task<UserDAL> GetAsync(string id)
        {
            var subresult = await _context.Users.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
            return MapTo<UserDAL>(subresult);
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            return await _manager.GetRolesAsync(userId).ConfigureAwait(false);
        }

        public async Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            return await _manager.GetClaimsAsync(userId).ConfigureAwait(false);
        }

        public async Task<IList<UserLoginDB>> GetLoginsAsync(string userId)
        {
            var logins = await _manager.GetLoginsAsync(userId).ConfigureAwait(false);
            IList<UserLoginDB> result = new List<UserLoginDB>(logins.Count);

            foreach (var login in logins)
            {
                result.Add(new UserLoginDB { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
            }
            return result;
        }

        public async Task ChangePasswordAsync(string userId, string newPassword, string oldPassword)
        {
            await _manager.ChangePasswordAsync(userId, oldPassword, newPassword).ConfigureAwait(false);
        }

        public async Task UpdateAsync(UserDAL user)
        {
            var userInDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id).ConfigureAwait(false);

            var entry = _context.Entry(userInDB);
            entry.CurrentValues.SetValues(user);
            entry.Property(u => u.Modified).CurrentValue = DateTime.UtcNow;

            entry.Property(u => u.Id).IsModified = false;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateEmailAsync(string userId, string newEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            if (user != null && await IsEmailAvailableAsync(newEmail).ConfigureAwait(false))
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
            bool userNameAvailable = !(await _context.Users.AnyAsync(u => u.UserName == newUserName).ConfigureAwait(false));

            if (userNameAvailable)
            {
                var user = _manager.FindById(userId);

                user.Modified = DateTime.UtcNow;
                user.UserName = newUserName;

                var entry = _context.Entry(user);

                entry.State = EntityState.Unchanged;
                entry.Property(u => u.UserName).IsModified = true;
                entry.Property(u => u.Modified).IsModified = true;

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return userNameAvailable;
        }

        // Business logic below
        public async Task AddFriendAsync(string userId, string friendId)
        {
            var user = await _context.Users.Include(p => p.Friends).FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            if (user == null)
            {
                return;
            }

            var friend = await _context.Users.FirstOrDefaultAsync(u => u.Id == friendId).ConfigureAwait(false);
            if (friend == null)
            {
                return;
            }

            user.Friends.Add(friend);
            user.Modified = DateTime.UtcNow;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IList<UserDAL>> GetFriendsAsync(string userId)
        {
            var user = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            return _mapper.Map<List<UserDAL>>(user.Friends);
        }

        public async Task DeleteFriendAsync(string userId, string deletingFriendId)
        {
            var user = await _context.Users.Include(p => p.Friends).FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            if(user == null)
            {
                return;
            }
            var friend = await _context.Users.FirstOrDefaultAsync(u => u.Id == deletingFriendId).ConfigureAwait(false);
            if (friend == null)
            {
                return;
            }

            user.Friends.Remove(friend);
            user.Modified = DateTime.UtcNow;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> AddProductAsync(string userId, string productId, int rating, string description)
        {
            var user = await _context.Users.Include(u => u.ProductDescriptions).FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            if (user == null)
            {
                return false;
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId).ConfigureAwait(false);
            if (product == null)
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
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<IList<UserProductDescriptionDB>> GetProductDescriptionsAsync(string userId)
        {
            var user = await _context.Users.Include(u => u.ProductDescriptions).FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            return user.ProductDescriptions;
        }

        public async Task<bool> UpdateProductDescriptionAsync(string userId, string productId, int rating, string description)
        {
            var descr = await _context.UserProductDescriptions.FirstOrDefaultAsync(d => d.UserId == userId && d.ProductId == productId).ConfigureAwait(false);
            if (descr == null)
            {
                return false;
            }

            descr.Rating = rating;
            descr.Description = description;
            descr.Modified = DateTime.UtcNow;

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DeleteProductAsync(string userId, string productId)
        {
            var description = await _context.UserProductDescriptions.FirstOrDefaultAsync(d => d.User.Id == userId && d.Product.Id == productId).ConfigureAwait(false);
            if (description != null)
            {
                _context.UserProductDescriptions.Remove(description);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            else return false;
        }

        public async Task ChangeGroupInvitationStatusAsync(string userId, string groupId, bool isConfirmed)
        {
            var groupUser = await _context.GroupUsers.FirstOrDefaultAsync(u => u.UserId == userId && u.GroupId == groupId).ConfigureAwait(false);
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
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<GroupUserDB>> GetGroupsAsync(string userId)
        {
            var user = await _context.Users.Include(u => u.Groups.Select(g => g.Group)).FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
            return user.Groups.Where(g => g.Group.IsNotDeleted);
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
