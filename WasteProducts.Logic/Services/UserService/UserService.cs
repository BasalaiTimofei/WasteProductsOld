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
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Mappings.UserMappings;

namespace WasteProducts.Logic.Services.UserService
{
    public class UserService : IUserService
    {
        private const string PASSWORD_RECOWERY_HEADER = "Запрос на восстановление пароля";

        private readonly IMailService _mailService;

        private readonly IUserRepository _userRepo;

        private readonly IRuntimeMapper _mapper;

        public UserService(IMailService mailService, IUserRepository userRepo)
        {
            _mailService = mailService;
            _userRepo = userRepo;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new UserClaimProfile());
                cfg.AddProfile(new UserLoginProfile());
                cfg.AddProfile(new ProductProfile());
            });
            _mapper = (new Mapper(config)).DefaultContext.Mapper;
        }

        public async Task<User> RegisterAsync(string email, string userName, string password, string passwordConfirmation)
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
                var userToAdd = MapTo<UserDB>(registeringUser);
                _userRepo.AddAsync(userToAdd).GetAwaiter().GetResult();
                var userDB = _userRepo.Select(email, false);
                var result = MapTo<User>(userDB);
                return result;
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
            return await Task.Run(async () =>
            {
                if (newPassword != newPasswordConfirmation || oldPassword != user.Password)
                {
                    return false;
                }
                user.Password = newPassword;

                await _userRepo.ResetPasswordAsync(MapTo<UserDB>(user), newPassword);
                return true;
            });
        }

        public async Task PasswordRequestAsync(string email)
        {
            try
            {
                User user = MapTo<User>(_userRepo.Select(email));

                if (user == null) return;
                // TODO придумать что писать в письме-восстановителе пароля и где хранить этот стринг
                await _mailService.SendAsync(email, PASSWORD_RECOWERY_HEADER, $"На ваш аккаунт \"Фуфлопродуктов\" был отправлен запрос на смену пароля. Напоминаем ваш пароль на сайте :\n\n{user.Password}\n\nВы можете поменять пароль в своем личном кабинете.");
            }
            catch { }
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepo.UpdateAsync(MapTo<UserDB>(user));
        }

        public async Task AddFriendAsync(User user, User newFriend)
        {
            if (!user.Friends.Contains(newFriend))
            {
                user.Friends.Add(newFriend);
                await _userRepo.AddFriendAsync(user.Id, newFriend.Id);
            }
        }

        public async Task AddProductAsync(User user, Product product)
        {
            if (user.Products.Count == 0 ||
                user.Products
                .FirstOrDefault(p => p.Barcode.Id == product.Barcode.Id &&
                                     p.Barcode.Code == product.Barcode.Code)
                                     != null)
            {
                user.Products.Add(product);
                await UpdateAsync(user);
            }
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            var userDB = MapTo<UserDB>(user);
            return await _userRepo.GetRolesAsync(userDB);
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
            await _userRepo.AddLoginAsync(MapTo<UserDB>(user), MapTo<UserLoginInfo>(login));
            user.Logins?.Add(login);
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

        public async Task DeleteProductAsync(User user, Product product)
        {
            if (user.Products.Contains(product))
            {
                user.Products.Remove(product);
                await UpdateAsync(user);
            }
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            await _userRepo.RemoveFromRoleAsync(MapTo<UserDB>(user), roleName);
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
            await _userRepo.RemoveLoginAsync(MapTo<UserDB>(user), MapTo<UserLoginInfo>(login));
            user.Logins?.Remove(login);
        }

        public async Task DeleteUserAsunc(User user)
        {
            await _userRepo.DeleteAsync(MapTo<UserDB>(user));
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
