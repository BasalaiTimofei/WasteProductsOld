using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Common.Services.MailService;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace WasteProducts.Logic.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMailService _mailService;

        private readonly IUserRepository _userRepo;

        private readonly IMapper _mapper;

        private bool _disposed;

        public UserService(IUserRepository userRepo, IMapper mapper, IMailService mailService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _mailService = mailService;
        }

        ~UserService()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _mailService?.Dispose();
                _userRepo?.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        public async Task RegisterAsync(string email, string userName, string password)
        {
            await Task.Run(async () =>
            {
                if (email == null ||
                userName == null ||
                password == null || 
                !_mailService.IsValidEmail(email) ||
                !(await _userRepo.IsEmailAvailableAsync(email)))
                {
                    return;
                }
                var registeringUser = new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    UserName = userName
                };
                var userToAdd = MapTo<UserDB>(registeringUser);
                await _userRepo.AddAsync(userToAdd, password);
            });
        }

        public async Task<User> LogInByEmailAsync(string email, string password)
        {
            return await Task.Run(async () =>
            {
                var userDB = await _userRepo.FindByEmailAndPasswordAsync(email, password);
                if (userDB == null)
                {
                    return null;
                }

                var loggedInUser = MapTo<User>(userDB);
                return loggedInUser;
            });
        }

        public async Task<User> LogInByNameAsync(string userName, string password)
        {
            return await Task.Run(async () =>
            {
                var userDB = await _userRepo.FindByNameAndPasswordAsync(userName, password);
                if (userDB == null)
                {
                    return null;
                }

                var loggedInUser = MapTo<User>(userDB);
                return loggedInUser;
            });
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            if (userId == null || oldPassword == null || newPassword == null)
            {
                return false;
            }

            return await Task.Run(async () =>
            {
                await _userRepo.ChangePasswordAsync(userId, newPassword, oldPassword);
                return true;
            });
        }

        // TODO переделать метод, он все равно будет отправлять захешированный пароль
        public Task ResetPasswordAsync(string email)
        {
            throw new NotImplementedException("Метод будет отправлять захешированный пароль, переделать метод, чтобы он отправлял ссылку на генерацию нового пароля");
        }

        //todo make async + userRepomethod
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await Task.Run(() =>
            {
                IEnumerable<UserDB> allUserDBs = _userRepo.GetSelector(true).ToList();
                var allUsers = _mapper.Map<IEnumerable<User>>(allUserDBs);
                return allUsers;
            });
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await Task.Run(async () =>
            {
                var userDB = await _userRepo.GetAsync(a => a.Id == id, true);
                var user = MapTo<User>(userDB);
                return user;
            });
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            return await _userRepo.GetRolesAsync(userId);
        }

        public async Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            return await _userRepo.GetClaimsAsync(userId);
        }

        public async Task<IList<UserLogin>> GetLoginsAsync(string userId)
        {
            IList<UserLoginDB> dbLogins = await _userRepo.GetLoginsAsync(userId);
            return _mapper.Map<IList<UserLogin>>(dbLogins);
            
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepo.UpdateAsync(MapTo<UserDB>(user));
        }

        public async Task<bool> UpdateEmailAsync(string userId, string newEmail)
        {
            if (userId == null || newEmail == null || !_mailService.IsValidEmail(newEmail))
            {
                return false;
            }

            bool result = await _userRepo.UpdateEmailAsync(userId, newEmail);
            return result;
        }

        public async Task<bool> UpdateUserNameAsync(string userId, string newUserName)
        {
            return await _userRepo.UpdateUserNameAsync(userId, newUserName);
        }

        public async Task AddFriendAsync(string userId, string newFriendId)
        {
            await _userRepo.AddFriendAsync(userId, newFriendId);
        }

        public async Task DeleteFriendAsync(string userId, string deletingFriendId)
        {
            await _userRepo.DeleteFriendAsync(userId, deletingFriendId);
        }

        public async Task<bool> AddProductAsync(string userId, string productId, int? rating, string description)
        {
            if (userId == null || productId == null || rating > 10 || rating < 0)
            {
                return false;
            }

            return await _userRepo.AddProductAsync(userId, productId, rating, description);
        }

        public async Task<bool> UpdateProductDescriptionAsync(string userId, string productId, int? rating, string description)
        {
            if (userId == null || productId == null || rating > 10 || rating < 0)
            {
                return false;
            }

            return await _userRepo.UpdateProductDescriptionAsync(userId, productId, rating, description);
        }

        public async Task<bool> DeleteProductAsync(string userId, string productId)
        {
            if (userId == null || productId == null)
            {
                return false;
            }

            return await _userRepo.DeleteProductAsync(userId, productId);
        }

        public async Task AddToRoleAsync(string userId, string roleName)
        {
            await _userRepo.AddToRoleAsync(userId, roleName);
        }

        public async Task AddClaimAsync(string userId, Claim claim)
        {
            await _userRepo.AddClaimAsync(userId, claim);
        }

        public async Task AddLoginAsync(string userId, UserLogin login)
        {
            await _userRepo.AddLoginAsync(userId, MapTo<UserLoginInfo>(login));
        }

        public async Task RemoveFromRoleAsync(string userId, string roleName)
        {
            await _userRepo.RemoveFromRoleAsync(userId, roleName);
        }

        public async Task RemoveClaimAsync(string userId, Claim claim)
        {
            await _userRepo.RemoveClaimAsync(userId, claim);
        }

        public async Task RemoveLoginAsync(string userId, UserLogin login)
        {
            await _userRepo.RemoveLoginAsync(userId, MapTo<UserLoginInfo>(login));
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _userRepo.DeleteAsync(userId);
        }

        private UserDB MapTo<T>(User user)
            =>
            _mapper.Map<UserDB>(user);

        private User MapTo<T>(UserDB user)
            =>
            _mapper.Map<User>(user);

        private UserLoginDB MapTo<T>(UserLogin user)
            =>
            _mapper.Map<UserLoginDB>(user);
    }
}
