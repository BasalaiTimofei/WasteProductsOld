using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IMailService _mailService;

        private readonly IUserRepository _userRepo;

        public UserService(IMailService mailService, IUserRepository userRepo)
        {
            _mailService = mailService;
            _userRepo = userRepo;
        }

        public void AddFriend(User user, User newFriend)
        {
            user.UserFriends.Add(newFriend);
            // TODO finish after mapping from User to UserDB is enabled _userRepo.Update(user);
            throw new NotImplementedException();
        }

        public void DeleteFriend(User user, User deletingFriend)
        {
            if (user.UserFriends.Contains(deletingFriend))
            {
                user.UserFriends.Remove(deletingFriend);
                // TODO finish after mapping from User to UserDB is enabled _userRepo.Update(user);
            }
            throw new NotImplementedException();
        }

        public bool LogIn(string email, string password, out User loggedInUser)
        {
            // TODO finish after mapping from User to UserDB is enabled loggedInUser = _userRepo.Select(email, password);
            throw new NotImplementedException();
        }

        public bool Register(string email, string password, string passwordConfirmation, out User registeredUser)
        {
            if(passwordConfirmation != password)
            {
                registeredUser = null;
                return false;
            }
            registeredUser = new User()
            {
                Email = email,
                Password = password,
            };
            // TODO finish after mapping from User to UserDB is enabled _userRepo.Add(user);
            throw new NotImplementedException();
        }

        public bool ResetPassword(User user, string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            throw new NotImplementedException();
        }

        public void ResetPasswordRequest(string email)
        {
            throw new NotImplementedException();
        }

        public bool ResetPasswordResponse(string newPassword, string newPasswordConfirmation)
        {
            throw new NotImplementedException();
        }

        public bool SetUserName(User user, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
