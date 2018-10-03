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
            if (!_mailService.IsValidEmail(email))
                throw new ValidationException("Plese provide valid Email.");

            if (!(await _repo.IsEmailAvailableAsync(email)) || !(await _repo.IsUserNameAvailable(userName)))
            {
                // throws 409 conflict
                throw new OperationCanceledException("Please provide unique Email and User Name.");
            }
            var (id, token) = await _repo.AddAsync(email, userName, password);
            if(path != null)
            {
                var fullpath = string.Format(path, id, token);
                await _mailService.SendAsync(email, UserResources.EmailConfirmationHeader, string.Format(UserResources.EmailConfirmationBody, fullpath));
            }
            return (id, token);
        }

        public Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            return _repo.ConfirmEmailAsync(userId, token);
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

        public Task ConfirmEmailChangingAsync(string userId, string token)
        {
            return _repo.ConfirmEmailChangingAsync(userId, token);
        }

        public async Task GenerateEmailChangingTokenAsync(string userId, string newEmail)
        {
            if (!_mailService.IsValidEmail(newEmail))
            {
                //throws 400
                throw new ValidationException("New email is not valid.");
            }
            else if (!(await _repo.IsEmailAvailableAsync(newEmail)))
            {
                // throws 409 conflict
                throw new OperationCanceledException("Please provide valid and unique Email.");
            }
            else
            {
                var token = await _repo.GenerateEmailChangingTokenAsync(userId, newEmail).ConfigureAwait(false);
                var fullpath = $@"http://localhost:2189/user/{userId}/confirmemailchanging/{token}";

                await _mailService.SendAsync(newEmail, UserResources.NewEmailConfirmationHeader, string.Format(UserResources.NewEmailConfirmationBody, fullpath));
            }
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

        public Task ResetPasswordAsync(string userId, string token, string newPassword)
        {
            return _repo.ResetPasswordAsync(userId, token, newPassword);
        }


        public Task<IEnumerable<User>> GetAllAsync()
        {
            return _repo.GetAllAsync()
                .ContinueWith(t => _mapper.Map<IEnumerable<User>>(t.Result));
        }

        public Task<User> GetAsync(string id)
        {
            return _repo.GetAsync(id)
                .ContinueWith(t => MapTo<User>(t.Result));
        }

        public Task<IList<string>> GetRolesAsync(string userId)
        {
            return _repo.GetRolesAsync(userId);
        }

        public Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            return _repo.GetClaimsAsync(userId);
        }

        public Task<IList<UserLogin>> GetLoginsAsync(string userId)
        {
            return _repo.GetLoginsAsync(userId)
                .ContinueWith(t => _mapper.Map<IList<UserLogin>>(t.Result));
        }

        public Task UpdateAsync(User user)
        {
            return _repo.UpdateAsync(MapTo<UserDB>(user));
        }

        public Task<bool> UpdateEmailAsync(string userId, string newEmail)
        {
            if (!_mailService.IsValidEmail(newEmail))
            {
                throw new ValidationException("Please follow validation rules.");
            }

            return _repo.UpdateEmailAsync(userId, newEmail);

        }

        public Task<bool> UpdateUserNameAsync(string userId, string newUserName)
        {
            return _repo.UpdateUserNameAsync(userId, newUserName);
        }

        public Task AddFriendAsync(string userId, string newFriendId)
        {
            return _repo.AddFriendAsync(userId, newFriendId);
        }

        public Task<IList<Friend>> GetFriendsAsync(string userId)
        {
            return _repo.GetFriendsAsync(userId)
                .ContinueWith(t => _mapper.Map<IList<Friend>>(t.Result));
        }

        public Task DeleteFriendAsync(string userId, string deletingFriendId)
        {
            return _repo.DeleteFriendAsync(userId, deletingFriendId);
        }

        public Task AddProductAsync(string userId, string productId, int rating, string description)
        {
            return _repo.AddProductAsync(userId, productId, rating, description);
        }

        public Task<IList<UserProduct>> GetProductsAsync(string userId)
        {
            return _repo.GetUserProductDescriptionsAsync(userId)
                .ContinueWith(t => _mapper.Map<IList<UserProduct>>(t.Result));
        }

        public Task UpdateProductDescriptionAsync(string userId, string productId, int rating, string description)
        {
            return _repo.UpdateProductDescriptionAsync(userId, productId, rating, description);
        }

        public Task DeleteProductAsync(string userId, string productId)
        {
            return _repo.DeleteProductAsync(userId, productId);
        }

        public Task<IEnumerable<GroupOfUser>> GetGroupsAsync(string userId)
        {
            return _repo.GetGroupsAsync(userId)
                .ContinueWith(t => _mapper.Map<IEnumerable<GroupOfUser>>(t.Result));
        }

        public Task RespondToGroupInvitationAsync(string userId, string groupId, bool isConfirmed)
        {
            return _repo.ChangeGroupInvitationStatusAsync(userId, groupId, isConfirmed);
        }

        public Task LeaveGroupAsync(string userId, string groupId)
        {
            return _repo.ChangeGroupInvitationStatusAsync(userId, groupId, false);
        }

        public Task AddToRoleAsync(string userId, string roleName)
        {
            return _repo.AddToRoleAsync(userId, roleName);
        }

        public Task AddClaimAsync(string userId, Claim claim)
        {
            return _repo.AddClaimAsync(userId, claim);
        }

        public Task AddLoginAsync(string userId, UserLogin login)
        {
            return _repo.AddLoginAsync(userId, MapTo<UserLoginDB>(login));
        }

        public Task RemoveFromRoleAsync(string userId, string roleName)
        {
            return _repo.RemoveFromRoleAsync(userId, roleName);
        }

        public Task RemoveClaimAsync(string userId, Claim claim)
        {
            return _repo.RemoveClaimAsync(userId, claim);
        }

        public Task RemoveLoginAsync(string userId, UserLogin login)
        {
            return _repo.RemoveLoginAsync(userId, MapTo<UserLoginDB>(login));
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
