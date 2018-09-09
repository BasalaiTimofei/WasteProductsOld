using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NLog;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.UserService;

namespace WasteProducts.Web.Controllers.Api.UserManagement
{
    /// <summary>
    /// API controller for user management.
    /// </summary>
    [RoutePrefix("api/user")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        private readonly ILogger _logger;

        /// <summary>
        /// Creates an Instance of UserController. User controller links frontend and business logic.
        /// </summary>
        /// <param name="userService">Instance of UserService from business logic.</param>
        /// <param name="logger">Instance of NLog Logger.</param>
        public UserController(IUserService userService, ILogger logger) : base(logger)
        {
            _userService = userService;
            _logger = logger;
        }

        //GET api/user
        /// <summary>
        /// Gets all users of the WasteProducts.
        /// </summary>
        /// <returns>IEnumerable of all the users.</returns>
        [HttpGet, Route("")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userService.GetAllUsersAsync();
        }

        // GET api/user/5
        /// <summary>
        /// Gets user by its ID.
        /// </summary>
        /// <param name="id">Id of a user.</param>
        /// <returns>User with the specific ID or null if there is no matches.</returns>
        [HttpGet, Route("{id}")]
        public async Task<User> GetUserById(string id)
        {
            //todo validation
            return await _userService.GetUserAsync(id);
        }

        /// <summary>
        /// Gets user by its email and password.
        /// </summary>
        /// <param name="email">Email of the user.</param>
        /// <param name="password">Password of the user.</param>
        /// <returns>User with the specific email and password or null if there is no matches.</returns>
        [HttpGet, Route("LoginByEmail/{email}_{password}")]
        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            return await _userService.LogInByEmailAsync(email, password);
        }

        /// <summary>
        /// Gets user by its name and password.
        /// </summary>
        /// <param name="userName">User's name.</param>
        /// <param name="password">Password of the user.</param>
        /// <returns>User with the specific email and password or null if there is no matches.</returns>
        [HttpGet, Route("LoginByUserName/{userName}_{password}")]
        public async Task<User> GetUserByNameAndPassword(string userName, string password)
        {
            return await _userService.LogInByNameAsync(userName, password);
        }

        /// <summary>
        /// Gets all the roles of the user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <returns>IList of roles of the user.</returns>
        [HttpGet, Route("Roles/{iserId}")]
        public async Task<IList<string>> GetRoles(string userId)
        {
            return await _userService.GetRolesAsync(userId);
        }

        /// <summary>
        /// Gets all claims of a user with the specific ID.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <returns>IList of claims of the user.</returns>
        [HttpGet, Route("Claims/{iserId}")]
        public async Task<IList<Claim>> GetClaims(string userId)
        {
            return await _userService.GetClaimsAsync(userId);
        }

        /// <summary>
        /// Gets all logins of a user with the specific ID.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <returns>IList of logins of the user.</returns>
        [HttpGet, Route("Logins/{userId}")]
        public async Task<IList<UserLogin>> GetLogins(string userId)
        {
            return await _userService.GetLoginsAsync(userId);
        }

        // POST api/User
        /// <summary>
        /// Registers a new user with the specific email, name and password.
        /// </summary>
        /// <param name="email">Email of the user. Should be unique for the application.</param>
        /// <param name="userName">NickName of the user. Should be unique for the application.</param>
        /// <param name="password">Password of the user</param>
        /// <returns></returns>
        [HttpPost, Route("register")]
        public async Task Register([FromBody] string email, [FromBody] string userName, [FromBody] string password)
        {
            //todo get from JSON, not User instance
            await _userService.RegisterAsync(email, userName, password);
        }

        // DELETE api/user/5
        /// <summary>
        /// Deletes user from the application.
        /// </summary>
        /// <param name="id">Id of the deleting user.</param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        public async Task Delete(string id)
        {
            await _userService.DeleteUserAsync(id);
        }

        // PUT api/user/5
        /// <summary>
        /// Updates user with the specific ID.
        /// </summary>
        /// <param name="id">ID of the updating user.</param>
        /// <param name="user">User to update.</param>
        /// <returns></returns>
        [HttpPut, Route("{id}")]
        public async Task Update(string id, [FromBody] User user)
        {
            await _userService.UpdateAsync(user);
        }

        // POST api/user/resetpassword
        /// <summary>
        /// Changes old password of the user with the specific ID to the new password.
        /// </summary>
        /// <param name="userId">ID of the user changing password.</param>
        /// <param name="oldPassword">Old password of the user.</param>
        /// <param name="newPassword">New password of the user.</param>
        /// <returns></returns>
        [HttpPut, Route("changePassword")]
        public async Task<bool> ChangePassword([FromBody] string userId, [FromBody] string oldPassword, [FromBody] string newPassword)
        {
            return await _userService.ChangePasswordAsync(userId, oldPassword, newPassword);
        }

        // POST api/user/resetpassword
        /// <summary>
        /// Asks for the email with a hyperlink which will reset password of the user with this email.
        /// </summary>
        /// <param name="email">Email of the user forgotten its password.</param>
        /// <returns></returns>
        [HttpPut, Route("ResetPassword")]
        public async Task ResetPassword([FromBody] string email)
        {
            await _userService.ResetPasswordAsync(email);
        }

        // POST api/user/updateemail
        /// <summary>
        /// Updates email of the user to the new email.
        /// </summary>
        /// <param name="userId">ID of the user changing its email.</param>
        /// <param name="newEmail">New email of the user.</param>
        /// <returns></returns>
        [HttpPut, Route("UpdateEmail")]
        public async Task<bool> UpdateEmail([FromBody] string userId, [FromBody] string newEmail)
        {
            return await _userService.UpdateEmailAsync(userId, newEmail);
        }

        //PUT api/user/UpdateUserName
        /// <summary>
        /// Updates user name of the user with the specific ID.
        /// </summary>
        /// <param name="userId">ID of the user changing its user name.</param>
        /// <param name="newUserName">A new user name for the user.</param>
        /// <returns></returns>
        [HttpPut, Route("UpdateUserName")]
        public async Task<bool> UpdateUserName([FromBody] string userId, [FromBody] string newUserName)
        {
            return await _userService.UpdateUserNameAsync(userId, newUserName);
        }

        // PUT api/User/Friends
        /// <summary>
        /// Adds a new friend to the friendlist of the user with the specific ID.
        /// </summary>
        /// <param name="userId">ID of the user looking for a new friend.</param>
        /// <param name="newFriendId">ID of a new friend of the user.</param>
        /// <returns></returns>
        [HttpPut, Route("AddFriend")]
        public async Task AddFriend([FromBody] string userId, [FromBody] string newFriendId)
        {
            await _userService.AddFriendAsync(userId, newFriendId);
        }

        //PUT api/User/Friends
        /// <summary>
        /// Deletes a friend with the specific friendId ID from the friendlist of the user with the userId ID.
        /// </summary>
        /// <param name="userId">ID of the user wanting to delete a friend from its friendlist.</param>
        /// <param name="friendId">ID of deleting friend.</param>
        /// <returns></returns>
        [HttpPut, Route("DeleteFriend")]
        public async Task DeleteFriend([FromBody]string userId, [FromBody] string friendId)
        {
            await _userService.DeleteFriendAsync(userId, friendId);
        }

        //PUT api/User/Products
        /// <summary>
        /// Adds product with its rating and description to the user's list of products.
        /// </summary>
        /// <param name="userId">ID of the user adding the product to its product list.</param>
        /// <param name="productId">ID of the adding product.</param>
        /// <param name="rating">Rating from 0 to 10 of this product.</param>
        /// <param name="description">User's own description of the product.</param>
        /// <returns></returns>
        [HttpPut, Route("AddProduct")]
        public async Task AddProduct([FromBody]string userId, [FromBody]string productId, [FromBody]int rating, [FromBody]string description)
        {
            await _userService.AddProductAsync(userId, productId, rating, description);
        }

        //POST api/User/Products/Delete
        /// <summary>
        /// Deletes product from the user's list of products.
        /// </summary>
        /// <param name="userId">ID of the user deleting the product from its list of products.</param>
        /// <param name="productId">ID of the deleting product.</param>
        /// <returns></returns>
        [HttpPut, Route("DeleteProduct")]
        public async Task DeleteProduct([FromBody]string userId, [FromBody]string productId)
        {
            await _userService.DeleteProductAsync(userId, productId);
        }

        /// <summary>
        /// Adds a user with the specific ID to the role.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        [HttpPut, Route("AddToRole")]
        public async Task AddToRole([FromBody] string userId, [FromBody]string roleName)
        {
            await _userService.AddToRoleAsync(userId, roleName);
        }

        /// <summary>
        /// Removes a user with the specific ID from the role.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        [HttpPut, Route("RemoveFromRole")]
        public async Task RemoveFromRole([FromBody] string userId, [FromBody]string roleName)
        {
            await _userService.RemoveFromRoleAsync(userId, roleName);
        }

        /// <summary>
        /// Adds a claim to the user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="claim">Claim to add to the user.</param>
        /// <returns></returns>
        [HttpPut, Route("AddClaim")]
        public async Task AddClaim([FromBody] string userId, [FromBody]Claim claim)
        {
            await _userService.AddClaimAsync(userId, claim);
        }

        /// <summary>
        /// Removes a claim from the user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="claim">Claim to remove from the user.</param>
        /// <returns></returns>
        [HttpPut, Route("RemoveClaim")]
        public async Task RemoveClaim([FromBody] string userId, [FromBody]Claim claim)
        {
            await _userService.RemoveClaimAsync(userId, claim);
        }

        /// <summary>
        /// Adds a login to the user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="userLogin">Login to add to the user.</param>
        /// <returns></returns>
        [HttpPut, Route("AddLogin")]
        public async Task AddLogin([FromBody] string userId, [FromBody]UserLogin userLogin)
        {
            await _userService.AddLoginAsync(userId, userLogin);
        }

        /// <summary>
        /// Removes a login to the user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="userLogin">Login to remove from the user.</param>
        /// <returns></returns>
        [HttpPut, Route("RemoveLogin")]
        public async Task RemoveLogin([FromBody] string userId, [FromBody]UserLogin userLogin)
        {
            await _userService.RemoveLoginAsync(userId, userLogin);
        }
    }
}