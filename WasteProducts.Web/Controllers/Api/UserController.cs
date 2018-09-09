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

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// API controller for user management.
    /// </summary>
    [RoutePrefix("UserService")]
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
        [Route("api/User/get")]
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
        [Route("{id:string}")]
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
        [Route("api/User/LoginByEmail")]
        public async Task<User> GetUserByEmailAndPassword([FromBody] string email, [FromBody] string password)
        {
            return await _userService.LogInByEmailAsync(email, password);
        }

        /// <summary>
        /// Gets user by its name and password.
        /// </summary>
        /// <param name="userName">User's name.</param>
        /// <param name="password">Password of the user.</param>
        /// <returns>User with the specific email and password or null if there is no matches.</returns>
        [Route("api/User/LoginByUserName")]
        public async Task<User> GetUserByNameAndPassword([FromBody] string userName, [FromBody] string password)
        {
            return await _userService.LogInByNameAsync(userName, password);
        }

        // POST api/User
        /// <summary>
        /// Registers a new user with the specific email, name and password.
        /// </summary>
        /// <param name="email">Email of the user. Should be unique for the application.</param>
        /// <param name="userName">NickName of the user. Should be unique for the application.</param>
        /// <param name="password">Password of the user</param>
        /// <returns></returns>
        [HttpPost, Route("api/User")]
        public async Task Register([FromBody] string email, [FromBody] string userName, [FromBody] string password)
        {
            //todo get from JSON, not User instance
            await _userService.RegisterAsync(email, userName, password);
        }

        // DELETE api/user/5
        /// <summary>
        /// Deletes user from the application.
        /// </summary>
        /// <param name="userId">Id of the deleting user.</param>
        /// <returns></returns>
        [HttpDelete, Route("{id:string}")]
        public async Task Delete(string userId)
        {
            await _userService.DeleteUserAsync(userId);
        }

        // PUT api/user/5
        /// <summary>
        /// Updates user with the specific ID.
        /// </summary>
        /// <param name="id">ID of the updating user.</param>
        /// <param name="user">User to update.</param>
        /// <returns></returns>
        [Route("{id:string}")]
        [HttpPut]
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
        [HttpPut, Route("api/user/changepassword")]
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
        [HttpPut, Route("api/User/ResetPassword")]
        public async Task ResetPassword(string email)
        {
            await _userService.ResetPasswordAsync(email);
        }

        // POST api/user/updateemail
        [HttpPut, Route("api/User/UpdateEmail")]
        public async Task<bool> UpdateEmail([FromBody] string userId, [FromBody] string newEmail)
        {
            return await _userService.UpdateEmailAsync(userId, newEmail);
        }

        //POST api/user/UpdateUserName
        [HttpPut, Route("api/User/UpdateUserName")]
        public async Task<bool> UpdateUserName([FromBody]User user, [FromBody]string newUserName)
        {
            return await _userService.UpdateUserNameAsync(user, newUserName);
        }

        // PUT api/User/Friends
        [HttpPut, Route("api/User/Friends")]
        public async Task AddFriend([FromBody]User user, [FromBody]User newFriend)
        {
            await _userService.AddFriendAsync(user, newFriend);
        }

        //POST api/User/Friends
        [HttpPut, Route("api/User/Friends/delete")]
        public async Task DeleteFriend([FromBody]User user, [FromBody]User friend)
        {
            await _userService.DeleteFriendAsync(user, friend);
        }

        //PUT api/User/Products
        [HttpPut, Route("api/User/Product")]
        public async Task AddProduct([FromBody]string userId, [FromBody]string productId, [FromBody]int rating, [FromBody]string description)
        {
            await _userService.AddProductAsync(userId, productId, rating, description);
        }

        //POST api/User/Products/Delete
        [HttpPut, Route("api/User/Products/Delete")]
        public async Task DeleteProduct([FromBody]string userId, [FromBody]string productId)
        {
            await _userService.DeleteProductAsync(userId, productId);
        }

        // POST api/User/Roles
        [HttpGet, Route("api/User/Roles")]
        public async Task<IList<string>> GetRoles([FromBody] string userId)
        {
            return await _userService.GetRolesAsync(userId);
        }

        // PUT api/User/Roles
        [HttpPut, Route("api/User/Roles")]
        public async Task AddToRole([FromBody] string userId, [FromBody]string roleName)
        {
            await _userService.AddToRoleAsync(userId, roleName);
        }

        // DELETE api/User/Roles
        [HttpPut, Route("api/User/Roles")]
        public async Task RemoveFromRole([FromBody] string userId, [FromBody]string role)
        {
            await _userService.RemoveFromRoleAsync(userId, role);
        }

        // PUT api/User/Claims
        [HttpPut, Route("api/User/Claims")]
        public async Task AddClaim([FromBody] string userId, [FromBody]Claim claim)
        {
            await _userService.AddClaimAsync(userId, claim);
        }

        //DELETE api/User/Claims
        [HttpPut, Route("api/User/Claims")]
        public async Task RemoveClaim([FromBody] string userId, [FromBody]Claim claim)
        {
            await _userService.RemoveClaimAsync(userId, claim);
        }

        // PUT api/User/Logins
        [HttpPut, Route("api/User/Logins")]
        public async Task AddLogin([FromBody] string userId, [FromBody]UserLogin userLogin)
        {
            await _userService.AddLoginAsync(userId, userLogin);
        }

        //DELETE api/User/Logins
        [HttpPut, Route("api/User/Logins")]
        public async Task RemoveLogin([FromBody] string userId, [FromBody]UserLogin userLogin)
        {
            await _userService.RemoveLoginAsync(userId, userLogin);
        }
    }
}