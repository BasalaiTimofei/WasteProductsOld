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
using Ninject;

namespace WasteProducts.Logic.Services.UserService
{
    public class UserService : IUserService
    {
        private const string PASSWORD_RECOWERY_HEADER = "Запрос на восстановление пароля";

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

        public async Task<User> RegisterAsync(string email, string userName, string password, string passwordConfirmation)
        {
            return await Task.Run(async () =>
            {
                User registeringUser = null;
                if (email == null || userName == null || password == null || passwordConfirmation == null ||
                    passwordConfirmation != password || !_mailService.IsValidEmail(email) || !(await _userRepo.IsEmailAvailableAsync(email)))
                {
                    return registeringUser;
                }
                registeringUser = new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    UserName = userName
                };
                var userToAdd = MapTo<UserDB>(registeringUser);
                await _userRepo.AddAsync(userToAdd, password);

                var userDB = _userRepo.Select(email, false);
                return MapTo<User>(userDB);
            });
        }

        public async Task<User> LogInAsync(string email, string password, bool getRoles = true)
        {
            return await Task.Run(() =>
            {
                User loggedInUser = null;
                var (userDB, roles) = _userRepo.Select(email, password);

                if (userDB == null)
                {
                    return loggedInUser;
                }

                loggedInUser = MapTo<User>(userDB);
                loggedInUser.Roles = roles;

                return loggedInUser;
            });
        }

        public async Task<bool> ResetPasswordAsync(User user, string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            return await Task.Run(async () =>
            {
                if (newPassword != newPasswordConfirmation)
                {
                    return false;
                }

                await _userRepo.ResetPasswordAsync(MapTo<UserDB>(user), newPassword, oldPassword);
                return true;
            });
        }

        // TODO переделать метод, он все равно будет отправлять захешированный пароль
        public Task ResetPasswordAsync(string email)
        {
            throw new NotImplementedException("Метод будет отправлять захешированный пароль, переделать метод, чтобы он отправлял ссылку на генерацию нового пароля");
        }

        //todo make async + userRepomethod
        public List<User> GetAllUsersInfo()
        {
            var listOfusers = new List<User>();
            var listOfuserDB = _userRepo.SelectAll();
            foreach (var userDB in listOfuserDB)
            {
                listOfusers.Add(MapTo<User>(userDB));
            }
            return listOfusers;
        }

        public async Task<User> GetUserInfo(string id)
        {
            return await Task.Run(() =>
            {
                var userDB = _userRepo.Select(a => a.Id == id, true);
                var user = MapTo<User>(userDB);
                //user.PasswordHash = "";
                return user;
            }
            );

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

        public async Task<bool> UpdateUserNameAsync(User user, string newUserName)
        {
            bool result = await _userRepo.UpdateUserNameAsync(MapTo<UserDB>(user), newUserName);
            if (result)
            {
                user.UserName = newUserName;
            }
            return result;
        }

        public async Task AddFriendAsync(User user, User newFriend)
        {
            if (!user.Friends.Contains(newFriend))
            {
                user.Friends.Add(newFriend);
                await _userRepo.AddFriendAsync(user.Id, newFriend.Id);
            }
        }

        public async Task DeleteFriendAsync(User user, User deletingFriend)
        {
            User delFriend = user.Friends.FirstOrDefault(u => u.Id == deletingFriend.Id);

            if (delFriend != null)
            {
                user.Friends.Remove(delFriend);
                await _userRepo.DeleteFriendAsync(user.Id, deletingFriend.Id);
            }
        }

        public async Task<bool> AddProductAsync(string userId, string productId, int rating, string description)
        {
            if (userId == null || productId == null || rating > 10 || rating < 0)
            {
                return false;
            }

            return await _userRepo.AddProductAsync(userId, productId, rating, description);
        }

        public async Task<bool> DeleteProductAsync(string userId, string productId)
        {
            if (userId == null || productId == null)
            {
                return false;
            }

            return await _userRepo.DeleteProductAsync(userId, productId);
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            var userDB = MapTo<UserDB>(user);
            return await _userRepo.GetRolesAsync(userDB);
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
