using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.Users;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.Users;
using WasteProducts.Logic.Common.Services.Mail;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using WasteProducts.Logic.Resources;

namespace WasteProducts.Logic.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IMailService _mailService;

        private readonly IUserRepository _repo;

        private readonly IMapper _mapper;

        private bool _disposed;

        public UserService(IUserRepository repo, IMapper mapper, IMailService mailService)
        {
            _repo = repo;
            _mapper = mapper;
            _mailService = mailService;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _mailService?.Dispose();
                _repo?.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        public async Task<(string id, string token)> RegisterAsync(string email, string userName, string password, string path)
        {
            if (email == null || userName == null || password == null ||
            !_mailService.IsValidEmail(email) || !(await _repo.IsEmailAvailableAsync(email)))
            {
                return(null, null);
            }
            var (id, token) = await _repo.AddAsync(email, userName, password);
            if(path != null)
            {
                var fullpath = string.Format(path, id, token);
                await _mailService.SendAsync(email, UserResources.EmailConfirmationHeader, string.Format(UserResources.EmailConfirmationBody, fullpath));
            }
            return (id, token);
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            return await _repo.ConfirmEmailAsync(userId, token);
        }

        public async Task<User> LogInByEmailAsync(string email, string password)
        {
            var userDB = await _repo.GetByEmailAndPasswordAsync(email, password);
            if (userDB == null)
            {
                return null;
            }

            var loggedInUser = MapTo<User>(userDB);
            return loggedInUser;
        }

        public async Task<User> LogInByNameAsync(string userName, string password)
        {
            var userDB = await _repo.GetByNameAndPasswordAsync(userName, password);
            if (userDB == null)
            {
                return null;
            }

            var loggedInUser = MapTo<User>(userDB);
            return loggedInUser;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            if (userId == null || oldPassword == null || newPassword == null)
            {
                return false;
            }

            await _repo.ChangePasswordAsync(userId, newPassword, oldPassword);
            return true;
        }

        public async Task<(string id, string token)> ResetPasswordRequestAsync(string email, string path)
        {
            if (email != null && path != null)
            {
                var (id, token) = await _repo.GeneratePasswordResetTokenAsync(email);
                var fullpath = string.Format(path, id, token);
                await _mailService.SendAsync(email, UserResources.ResetPasswordHeader, string.Format(UserResources.ResetPasswordBody, fullpath));
                return (id, token);
            }
            else
            {
                return (null, null);
            }
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            return await _repo.ResetPasswordAsync(userId, token, newPassword);
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            IEnumerable<UserDAL> allUserDBs = await _repo.GetAllAsync(true);
            var allUsers = _mapper.Map<IEnumerable<User>>(allUserDBs);
            return allUsers;
        }

        public async Task<User> GetUserAsync(string id)
        {
            var userDB = await _repo.GetAsync(id, true);
            var user = MapTo<User>(userDB);
            return user;
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            return await _repo.GetRolesAsync(userId);
        }

        public async Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            return await _repo.GetClaimsAsync(userId);
        }

        public async Task<IList<UserLogin>> GetLoginsAsync(string userId)
        {
            IList<UserLoginDB> dbLogins = await _repo.GetLoginsAsync(userId);
            return _mapper.Map<IList<UserLogin>>(dbLogins);
            
        }

        public async Task UpdateAsync(User user)
        {
            await _repo.UpdateAsync(MapTo<UserDB>(user));
        }

        public async Task<bool> UpdateEmailAsync(string userId, string newEmail)
        {
            if (userId == null || newEmail == null || !_mailService.IsValidEmail(newEmail))
            {
                return false;
            }

            bool result = await _repo.UpdateEmailAsync(userId, newEmail);
            return result;
        }

        public async Task<bool> UpdateUserNameAsync(string userId, string newUserName)
        {
            return await _repo.UpdateUserNameAsync(userId, newUserName);
        }

        public Task AddFriendAsync(string userId, string newFriendId)
        {
            return _repo.AddFriendAsync(userId, newFriendId);
        }

        public Task<IList<Friend>> GetFriendsAsync(string userId)
        {
            return _repo.GetFriendsAsync(userId).ContinueWith(t => _mapper.Map<IList<Friend>>(t.Result));
        }

        public Task DeleteFriendAsync(string userId, string deletingFriendId)
        {
            return _repo.DeleteFriendAsync(userId, deletingFriendId);
        }

        public async Task<bool> AddProductAsync(string userId, string productId, int rating, string description)
        {
            if (userId == null || productId == null || rating > 10 || rating < 0)
            {
                return false;
            }

            return await _repo.AddProductAsync(userId, productId, rating, description);
        }

        public Task<IList<ProductDescription>> GetProductDescriptionsAsync(string userId)
        {
            return _repo.GetProductDescriptionsAsync(userId).ContinueWith(t => _mapper.Map<IList<ProductDescription>>(t.Result));
        }

        public async Task<bool> UpdateProductDescriptionAsync(string userId, string productId, int rating, string description)
        {
            if (userId == null || productId == null || rating > 10 || rating < 0)
            {
                return false;
            }

            return await _repo.UpdateProductDescriptionAsync(userId, productId, rating, description);
        }

        public async Task<bool> DeleteProductAsync(string userId, string productId)
        {
            if (userId == null || productId == null)
            {
                return false;
            }

            return await _repo.DeleteProductAsync(userId, productId);
        }

        public async Task AddToRoleAsync(string userId, string roleName)
        {
            await _repo.AddToRoleAsync(userId, roleName);
        }

        public async Task AddClaimAsync(string userId, Claim claim)
        {
            await _repo.AddClaimAsync(userId, claim);
        }

        public async Task AddLoginAsync(string userId, UserLogin login)
        {
            await _repo.AddLoginAsync(userId, MapTo<UserLoginDB>(login));
        }

        public async Task RemoveFromRoleAsync(string userId, string roleName)
        {
            await _repo.RemoveFromRoleAsync(userId, roleName);
        }

        public async Task RemoveClaimAsync(string userId, Claim claim)
        {
            await _repo.RemoveClaimAsync(userId, claim);
        }

        public async Task RemoveLoginAsync(string userId, UserLogin login)
        {
            await _repo.RemoveLoginAsync(userId, MapTo<UserLoginDB>(login));
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _repo.DeleteAsync(userId);
        }

        private UserDAL MapTo<T>(User user)
            =>
            _mapper.Map<UserDAL>(user);

        private User MapTo<T>(UserDAL user)
            =>
            _mapper.Map<User>(user);

        private UserLoginDB MapTo<T>(UserLogin user)
            =>
            _mapper.Map<UserLoginDB>(user);

        ~UserService()
        {
            Dispose();
        }
    }
}
