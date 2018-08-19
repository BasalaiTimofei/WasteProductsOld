using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Common.Services.MailService;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace WasteProducts.Logic.Services.UserService
{
    public class UserService : IUserService
    {
        private const string PASSWORD_RECOWERY_HEADER = "Запрос на восстановление пароля";

        private readonly IMailService _mailService;

        private readonly IUserRepository _userRepo;

        public UserService(IMailService mailService, IUserRepository userRepo)
        {
            _mailService = mailService;
            _userRepo = userRepo;
        }

        public async Task<User> RegisterAsync(string email, string password, string userName, string passwordConfirmation)
        {
            return await Task.Run(() =>
            {
                User registeringUser = null;
                if (passwordConfirmation != password || !_mailService.IsValidEmail(email))
                {
                    return registeringUser;
                }
                registeringUser = new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    Password = password,
                    UserName = userName
                };

                _userRepo.AddAsync(MapTo<UserDB>(registeringUser)).GetAwaiter().GetResult();
                return MapTo<User>(_userRepo.Select(email, false));
            });
        }

        public async Task<User> LogInAsync(string email, string password, bool getRoles = true)
        {
            return await Task.Run(() =>
            {
                User loggedInUser = null;
                if (getRoles)
                {
                    var (userDB, roles) = _userRepo.SelectWithRoles(u => u.Email == email && u.PasswordHash == password, false);
                    if (userDB == null)
                    {
                        return loggedInUser;
                    }
                    loggedInUser = MapTo<User>(userDB);
                    loggedInUser.Roles = roles;
                }
                else
                {
                    UserDB userDB = _userRepo.Select(email, password, false);
                    if (userDB == null)
                    {
                        return loggedInUser;
                    }
                    loggedInUser = MapTo<User>(userDB);
                }

                return loggedInUser;
            });
        }

        public async Task<bool> ResetPasswordAsync(User user, string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            return await Task.Run(() =>
            {
                if (newPassword != newPasswordConfirmation || oldPassword != user.Password)
                {
                    return false;
                }
                user.Password = newPassword;

                UpdateAsync(user).GetAwaiter().GetResult();
                return true;
            });
        }

        public async Task PasswordRequestAsync(string email)
        {
            try
            {
                User user = MapTo<User>(_userRepo.Select(email));

                // TODO придумать что писать в письме-восстановителе пароля и где хранить этот стринг
                await _mailService.SendAsync(email, PASSWORD_RECOWERY_HEADER, $"На ваш аккаунт \"Фуфлопродуктов\" был отправлен запрос на смену пароля. Напоминаем ваш пароль на сайте :\n\n{user.Password}\n\nВы можете поменять пароль в своем личном кабинете.");
            }
            catch { }
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepo.UpdateAsync(MapTo<UserDB>(user));
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await _userRepo.GetRolesAsync(MapTo<UserDB>(user));
        }

        public async Task AddToRoleAsync(User user, string roleName)
        {
            await _userRepo.AddToRoleAsync(MapTo<UserDB>(user), roleName);
            user.Roles.Add(roleName);
        }

        public async Task AddClaimAsync(User user, Claim claim)
        {
            await _userRepo.AddClaimAsync(MapTo<UserDB>(user), claim);
            user.Claims.Add(claim);
        }

        public async Task AddLoginAsync(User user, UserLogin login)
        {
            await _userRepo.AddLoginAsync(MapTo<UserDB>(user), Mapper.Map<UserLoginInfo>(login));
            user.Logins?.Add(login);
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            await _userRepo.RemoveFromRoleAsync(MapTo<UserDB>(user), roleName);
            user.Roles?.Remove(roleName);
        }

        public async Task RemoveClaimAsync(User user, Claim claim)
        {
            await _userRepo.RemoveClaimAsync(MapTo<UserDB>(user), claim);
            user.Claims?.Remove(claim);
        }

        public async Task RemoveLoginAsync(User user, UserLogin login)
        {
            await _userRepo.RemoveLoginAsync(MapTo<UserDB>(user), Mapper.Map<UserLoginInfo>(login));
            user.Logins?.Remove(login);
        }

        public void AddFriend(User user, User newFriend)
        {
            user.UserFriends.Add(newFriend);
            UpdateAsync(user).GetAwaiter().GetResult();
        }

        public void DeleteFriend(User user, User deletingFriend)
        {
            if (user.UserFriends.Contains(deletingFriend))
            {
                user.UserFriends.Remove(deletingFriend);
                UpdateAsync(user).GetAwaiter().GetResult();
            }
        }

        private UserDB MapTo<T>(User user)
            where T : UserDB
            =>
            Mapper.Map<UserDB>(user);

        private User MapTo<T>(UserDB user)
            where T : User
            =>
            Mapper.Map<User>(user);
    }
}
