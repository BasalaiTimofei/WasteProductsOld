using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class UserService : IUserService
    {
        public void AddFriend(User user, User newFriend)
        {
            throw new NotImplementedException();
        }

        public void DeleteFriend(User user, User deletingFriend)
        {
            throw new NotImplementedException();
        }

        public bool LogIn(string email, string password, out User loggedInUser)
        {
            throw new NotImplementedException();
        }

        public bool Register(string email, string password, string passwordConfirmation, out User registeredUser)
        {
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
