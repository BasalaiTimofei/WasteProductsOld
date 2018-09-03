using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Repositories.UserManagement
{
    /// <summary>
    /// Standart DAL level interface using to make CRUD operations with User models.
    /// </summary>
    public interface IUserRepository : IDisposable
    {
        /// <summary>
        /// Adds new registered user in the repository.
        /// </summary>
        /// <param name="user">New registered user to add.</param>
        /// <param name="password">Password of the new user.</param>
        /// <returns></returns>
        Task AddAsync(UserDB user, string password);

        /// <summary>
        /// Checks if email wasn't used in registering any user. Returns true if not.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> IsEmailAvailableAsync(string email);

        /// <summary>
        /// Add a claim to a user.
        /// </summary>
        /// <param name="user">Specific claim will be added to the user.</param>
        /// <param name="claim">Specific claim to add to the user.</param>
        /// <returns></returns>
        Task AddClaimAsync(UserDB user, Claim claim);

        /// <summary>
        ///  Add a login to the user.
        /// </summary>
        /// <param name="user">Specific login will be added to the user.</param>
        /// <param name="login">Specific login to add to the user.</param>
        /// <returns></returns>
        Task AddLoginAsync(UserDB user, UserLoginDB login);

        /// <summary>
        /// Add a user to a role.
        /// </summary>
        /// <param name="user">User will be added to this specific role.</param>
        /// <param name="roleName">Name of the specific role to add to the user.</param>
        /// <returns></returns>
        Task AddToRoleAsync(UserDB user, string roleName);

        /// <summary>
        /// Adds user with id = friendId to the list of friends of user with Id = userId.
        /// </summary>
        /// <param name="userId">Id of user which friend list will be expanded.</param>
        /// <param name="friendId">Id of a new friend of the user with Id = userId</param>
        /// <returns></returns>
        Task AddFriendAsync(string userId, string friendId);

        /// <summary>
        /// Deletes the record of the specific user.
        /// </summary>
        /// <param name="userId">ID of the specific user to delete.</param>
        Task DeleteAsync(string userId);

        /// <summary>
        /// Remove a claim from a user.
        /// </summary>
        /// <param name="user">Specific claim will be removed from the user.</param>
        /// <param name="claim">Specific claim to remove from the user.</param>
        /// <returns></returns>
        Task RemoveClaimAsync(UserDB user, Claim claim);

        /// <summary>
        /// Remove a user from a role.
        /// </summary>
        /// <param name="user">User will be removed from this specific role.</param>
        /// <param name="roleName">Name of the specific role to remove from the user.</param>
        /// <returns></returns>
        Task RemoveFromRoleAsync(UserDB user, string roleName);

        /// <summary>
        /// Remove a login from a user.
        /// </summary>
        /// <param name="user">Specific login will be removed from the user.</param>
        /// <param name="login">Specific login to remove from the user.</param>
        /// <returns></returns>
        Task RemoveLoginAsync(UserDB user, UserLoginDB login);

        /// <summary>
        /// Deletes a specific friend from the specific user's friend list.
        /// </summary>
        /// <param name="userId">From the list of friends of UserDB with Id = userID the UserDB with Id = deletingFriendId will be deleted.</param>
        /// <param name="deletingFriendId">Specific friend's Id to delete from the user's friend list.</param>
        Task DeleteFriendAsync(string userID, string deletingFriendId);

        /// <summary>
        /// Returns first registered user matches the predicate or null if there is no matches.
        /// </summary>
        /// <param name="predicate">Filter for the users.</param>
        /// <param name="lazyInitiation">True if you don't want to initiate navigation properties, otherwise, false.</param>
        /// <returns>The first registered user matches the predicate or null if there is no matches.</returns>
        UserDB Select(Func<UserDB, bool> predicate, bool lazyInitiation = true);

        /// <summary>
        /// Returns a registered user by its email.
        /// </summary>
        /// <param name="email">Email of the requested user.</param>
        /// <returns></returns>
        UserDB Select(string email, bool lazyInitiation = true);

        /// <summary>
        /// Returns a registered user by its email and password.
        /// </summary>
        /// <param name="email">Email of the requested user.</param>
        /// <param name="password">Password of the requested user.</param>
        /// <returns>User with the specific email and password.</returns>
        UserDB Select(string email, string password, bool lazyInitiation = true);

        (UserDB, IList<string>) Select(string email, string password);

        /// <summary>
        /// Returns first registered user matches the predicate with user's roles (or two nulls if there is no matches).
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="lazyInitiation"></param>
        /// <returns></returns>
        (UserDB user, IList<string> roles) SelectWithRoles(Func<UserDB, bool> predicate, bool lazyInitiation = true);

        /// <summary>
        /// Returns all registered users in an enumerable.
        /// </summary>
        /// <returns>All the registered users.</returns>
        List<UserDB> SelectAll();

        /// <summary>
        /// Returns the users filtered by the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<UserDB> SelectRange(Func<UserDB, bool> predicate, bool lazyInitiation = true);

        /// <summary>
        /// Get the names of the roles a user is a member of.
        /// </summary>
        /// <param name="user">Method will return roles of this user.</param>
        /// <returns></returns>
        Task<IList<string>> GetRolesAsync(UserDB user);

        /// <summary>
        /// Resets password of a user.
        /// </summary>
        /// <param name="user">Password of this user will be reset.</param>
        /// <param name="newPassword">New password for a user.</param>
        /// <returns></returns>
        Task ResetPasswordAsync(UserDB user, string newPassword, string oldPassword);

        /// <summary>
        /// Updates the record of the specific user.
        /// </summary>
        /// <param name="user">Specific user to update.</param>
        Task UpdateAsync(UserDB user);

        /// <summary>
        /// Updates user's email if it isn't used by another user. Returns true if email was successfully updated.
        /// </summary>
        /// <param name="userId">ID of User wanting to update its Email.</param>
        /// <param name="newEmail">New unique email.</param>
        /// <returns></returns>
        Task<bool> UpdateEmailAsync(string userId, string newEmail);

        /// <summary>
        /// Updates user's UserName if it isn't used by another user. Returns true if UserName was successfully updated.
        /// </summary>
        /// <param name="user">User wanting to update its UserName.</param>
        /// <param name="newUserName">New unique UserName</param>
        /// <returns></returns>
        Task<bool> UpdateUserNameAsync(UserDB user, string newUserName);
    }
}