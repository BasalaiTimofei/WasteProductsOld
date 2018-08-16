using Microsoft.AspNet.Identity;
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
    public interface IUserRepository
    {
        /// <summary>
        /// Adds new registered user in the repository.
        /// </summary>
        /// <param name="user">New registered user to add.</param>
        Task AddAsync(UserDB user);

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
        Task AddLoginAsync(UserDB user, UserLoginInfo login);

        /// <summary>
        /// Add a user to a role.
        /// </summary>
        /// <param name="user">User will be removed from this specific role.</param>
        /// <param name="roleName">Name of the specific role to add to the user.</param>
        /// <returns></returns>
        Task AddToRoleAsync(UserDB user, string roleName);

        /// <summary>
        /// Deletes the record of the specific user.
        /// </summary>
        /// <param name="user">Specific user to delete.</param>
        Task DeleteAsync(UserDB user);

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
        Task RemoveLoginAsync(UserDB user, UserLoginInfo login);

        /// <summary>
        /// Returns first registered user matches the predicate.
        /// </summary>
        /// <param name="email">Email of the requested user.</param>
        /// <returns></returns>
        UserDB Select(Func<UserDB, bool> predicate);

        /// <summary>
        /// Returns a registered user by its email.
        /// </summary>
        /// <param name="email">Email of the requested user.</param>
        /// <returns></returns>
        UserDB Select(string email);

        /// <summary>
        /// Returns a registered user by its email and password.
        /// </summary>
        /// <param name="email">Email of the requested user.</param>
        /// <param name="password">Password of the requested user.</param>
        /// <returns>User with the specific email and password.</returns>
        UserDB Select(string email, string password);

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
        List<UserDB> SelectRange(Func<UserDB, bool> predicate);

        /// <summary>
        /// Updates the record of the specific user.
        /// </summary>
        /// <param name="user">Specific user to update.</param>
        Task UpdateAsync(UserDB user);
    }
}