using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Services
{
    public interface IUserService
    {
        bool Register(string email, string password, string passwordConfirmation, out User registeredUser);

        bool LogIn(string email, string password, out User loggedInUser);

        bool ResetPassword(User user, string oldPassword, string newPassword, string newPasswordConfirmation);

        void ResetPasswordQuery(string email);

        bool ResetPasswordResponse(string newPassword, string newPasswordConfirmation);

        bool SetUserName(User user, string userName);

        void AddFriend(User newFriend);

        void DeleteFriend(User deletingFriend);

        // TODO USER MANAGEMENT PENDING FUNCTIONAL TO ADD:
        // sharing my products with my friends after model "Product" is created
        // subscribing special users to watch their news (if this functional will be approved)
        // chatting between users
        // registering by Facebook and VK profiles
        // getting "Approved Representative of The Company" status and its unique functional like special tools for speed feedback

    }
}
