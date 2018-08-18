using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories.UserManagement;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Logic.Common.Services.MailService;
using WasteProducts.Logic.Mappings.UserMappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Services.UserService
{
    public class UserService : IUserService
    {
        private const string PASSWORD_RECOWERY_HEADER = "Запрос на восстановление пароля";

        private readonly IMailService _mailService;

        private readonly IUserRepository _userRepo;

        static UserService()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new UserProfile()));
        }

        public UserService(IMailService mailService, IUserRepository userRepo)
        {
            _mailService = mailService;
            _userRepo = userRepo;
        }

        public void AddFriend(User user, User newFriend)
        {
            user.UserFriends.Add(newFriend);
            UpdateUser(user);
        }

        public void DeleteFriend(User user, User deletingFriend)
        {
            if (user.UserFriends.Contains(deletingFriend))
            {
                user.UserFriends.Remove(deletingFriend);
                UpdateUser(user);
            }
        }

        public bool LogIn(string email, string password, out User loggedInUser, bool getRoles = true)
        {
            if (getRoles)
            {
                var (userDB, roles) = _userRepo.SelectWithRoles(u => u.Email == email && u.PasswordHash == password, false);
                if(userDB == null)
                {
                    loggedInUser = null;
                    return false;
                }
                loggedInUser = Mapper.Map<User>(userDB);
                loggedInUser.Roles = roles;
            }
            else
            {
                UserDB userDB = _userRepo.Select(email, password, false);
                if (userDB == null)
                {
                    loggedInUser = null;
                    return false;
                }
                loggedInUser = Mapper.Map<User>(_userRepo.Select(email, password, false));
            }

            return loggedInUser != null;
        }

        public bool Register(string email, string password, string passwordConfirmation, out User registeredUser)
        {
            if(passwordConfirmation != password || !_mailService.IsValidEmail(email))
            {
                registeredUser = null;
                return false;
            }
            registeredUser = new User()
            {
                Email = email,
                Password = password,
            };
            var userDb = Mapper.Map<UserDB>(registeredUser);
            _userRepo.AddAsync(userDb);
            return true;
        }

        public bool ResetPassword(User user, string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            if(newPassword != newPasswordConfirmation || oldPassword != user.Password)
            {
                return false;
            }
            user.Password = newPassword;

            UpdateUser(user);
            return true;
        }

        public bool PasswordRequest(string email)
        {
            var user = Mapper.Map<User>(_userRepo.Select(email));
            if (user == null)
            {
                return false;
            }

            // TODO придумать что писать в письме-восстановителе пароля и где хранить этот стринг
            _mailService.Send(email, PASSWORD_RECOWERY_HEADER, $"На ваш аккаунт \"Фуфлопродуктов\" был отправлен запрос на смену пароля. Напоминаем ваш пароль на сайте :\n\n{user.Password}\n\nВы можете поменять пароль в своем личном кабинете.");
            return true;
        }

        public void UpdateUserInfo(User user)
        {
            UpdateUser(user);
        }

        private void UpdateUser(User user)
        {
            var userDb = Mapper.Map<UserDB>(user);
            _userRepo.UpdateAsync(userDb);
        }

        public async Task AddToRoleAsync(User user, string roleName)
        {
            await _userRepo.AddToRoleAsync(Mapper.Map<UserDB>(user), roleName);
            user.Roles.Add(roleName);
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await _userRepo.GetRolesAsync(Mapper.Map<UserDB>(user));
        }
    }
}
