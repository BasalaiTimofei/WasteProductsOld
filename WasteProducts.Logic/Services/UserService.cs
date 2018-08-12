using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Mappings;

namespace WasteProducts.Logic.Services
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
            var userDb = Mapper.Map<UserDB>(user);
            _userRepo.Update(userDb);
        }

        public void DeleteFriend(User user, User deletingFriend)
        {
            if (user.UserFriends.Contains(deletingFriend))
            {
                user.UserFriends.Remove(deletingFriend);
                var userDb = Mapper.Map<UserDB>(user);
                _userRepo.Update(userDb);
            }
        }

        public bool LogIn(string email, string password, out User loggedInUser)
        {
            loggedInUser = Mapper.Map<User>(_userRepo.Select(email, password));

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
            _userRepo.Add(userDb);
            return true;
        }

        public bool ResetPassword(User user, string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            if(newPassword != newPasswordConfirmation || oldPassword != user.Password)
            {
                return false;
            }
            user.Password = newPassword;

            var userDb = Mapper.Map<UserDB>(user);
            _userRepo.Update(userDb);
            return true;
        }

        public void PasswordRequest(string email)
        {
            // TODO придумать чо писать в письме-восстановителе пароля и где хранить этот стринг
            var user = Mapper.Map<User>(_userRepo.Select(email));
            if (user == null)
            {
                return;
            }

            _mailService.Send(email, PASSWORD_RECOWERY_HEADER, $"На ваш аккаунт \"Фуфлопродуктов\" был отправлен запрос на смену пароля. Напоминаем ваш пароль на сайте :\n\n{user.Password}\n\nВы можете поменять пароль в своем личном кабинете.");
        }

        public bool SetUserName(User user, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
