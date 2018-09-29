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
using FluentValidation;

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
            if (!_mailService.IsValidEmail(email) || !(await _repo.IsEmailAvailableAsync(email)))
            {
                // throws 409 conflict
                throw new OperationCanceledException("Please provide valid and unique Email.");
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

            var loggedInUser = MapTo<User>(userDB);
            return loggedInUser;
        }

        public async Task<User> LogInByNameAsync(string userName, string password)
        {
            var userDB = await _repo.GetByNameAndPasswordAsync(userName, password);

            var loggedInUser = MapTo<User>(userDB);
            return loggedInUser;
        }

        public Task ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            return _repo.ChangePasswordAsync(userId, newPassword, oldPassword);
        }

        public async Task<(string id, string token)> ResetPasswordRequestAsync(string email, string path)
        {
            if (path != null)
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

        public async Task ResetPasswordAsync(string userId, string token, string newPassword)
        {
            await _repo.ResetPasswordAsync(userId, token, newPassword);
        }


        public async Task<IEnumerable<User>> GetAllAsync()
        {
            IEnumerable<UserDAL> allUserDBs = await _repo.GetAllAsync();
            var allUsers = _mapper.Map<IEnumerable<User>>(allUserDBs);
            return allUsers;
        }

        public async Task<User> GetAsync(string id)
        {
            var userDB = await _repo.GetAsync(id);
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
            if (!_mailService.IsValidEmail(newEmail))
            {
                throw new ValidationException("Please follow validation rules.");
            }

            return await _repo.UpdateEmailAsync(userId, newEmail);
        }

        public async Task<bool> UpdateUserNameAsync(string userId, string newUserName)
        {
            return await _repo.UpdateUserNameAsync(userId, newUserName);
        }

        public async Task AddFriendAsync(string userId, string newFriendId)
        {
            await _repo.AddFriendAsync(userId, newFriendId);
        }

        public Task<IList<Friend>> GetFriendsAsync(string userId)
        {
            return _repo.GetFriendsAsync(userId).ContinueWith(t => _mapper.Map<IList<Friend>>(t.Result));
        }

        public async Task DeleteFriendAsync(string userId, string deletingFriendId)
        {
            await _repo.DeleteFriendAsync(userId, deletingFriendId);
        }

        public async Task AddProductAsync(string userId, string productId, int rating, string description)
        {
            await _repo.AddProductAsync(userId, productId, rating, description);
        }

        public Task<IList<UserProduct>> GetProductsAsync(string userId)
        {
            return _repo.GetUserProductDescriptionsAsync(userId).ContinueWith(t => _mapper.Map<IList<UserProduct>>(t.Result));
        }

        public async Task UpdateProductDescriptionAsync(string userId, string productId, int rating, string description)
        {
            await _repo.UpdateProductDescriptionAsync(userId, productId, rating, description);
        }

        public async Task DeleteProductAsync(string userId, string productId)
        {
            await _repo.DeleteProductAsync(userId, productId);
        }

        public Task<IEnumerable<GroupOfUser>> GetGroupsAsync(string userId)
        {
            return _repo.GetGroupsAsync(userId).ContinueWith(t => _mapper.Map<IEnumerable<GroupOfUser>>(t.Result));
        }

        public async Task RespondToGroupInvitationAsync(string userId, string groupId, bool isConfirmed)
        {
            await _repo.ChangeGroupInvitationStatusAsync(userId, groupId, isConfirmed);
        }

        public Task LeaveGroupAsync(string userId, string groupId)
        {
            return _repo.ChangeGroupInvitationStatusAsync(userId, groupId, false);
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

        public Task DeleteUserAsync(string userId)
        {
            return _repo.DeleteAsync(userId);
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
