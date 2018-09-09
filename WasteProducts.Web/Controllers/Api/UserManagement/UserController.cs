using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.UserService;
using WasteProducts.Web.Models.Users;

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
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Users are found.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There are no Users.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the search.")]
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
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "User is found.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There are no User with such ID.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the search.")]
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
        [HttpPost, Route("LoginByEmail")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "User was successfully logged in.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Please provide correct Email and Password.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "Already logged in.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the logging in.")]
        public async Task<User> LoginByEmailAndPassword([FromBody]LoginUser user)
        {
            return await _userService.LogInByEmailAsync(user.UserNameOREmail, user.Password);
        }

        /// <summary>
        /// Gets user by its name and password.
        /// </summary>
        /// <param name="userName">User's name.</param>
        /// <param name="password">Password of the user.</param>
        /// <returns>User with the specific email and password or null if there is no matches.</returns>
        [HttpGet, Route("LoginByUserName")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "User was successfully logged in.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Please provide correct UserName and Password.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "Already logged in.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the logging in.")]
        public async Task<User> LoginByNameAndPassword([FromBody]LoginUser user)
        {
            return await _userService.LogInByNameAsync(user.UserNameOREmail, user.Password);
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
        /// <param name="user">User model that contains: Email, UserName and Password.</param>
        /// <returns></returns>
        [HttpPost, Route("register")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "User was successfully registered.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "Please provide unique UserName and Email.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the registration.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        public async Task Register([FromBody] RegisterUser user)
        {
            await _userService.RegisterAsync(user.Email, user.UserName, user.Password);
        }

        // DELETE api/user/5
        /// <summary>
        /// Deletes user from the application.
        /// </summary>
        /// <param name="id">Id of the deleting user.</param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "User is deleted.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no such User.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the deletion.")]
        public async Task Delete([FromUri] string id)
        {
            await _userService.DeleteUserAsync(id);
        }

        // PUT api/user/5
        // TODO change update User to concrete updates
        /*
        [HttpPut, Route("{id}")]
        public async Task Update([FromUri] string id, [FromBody] User user)
        {
            await _userService.UpdateAsync(user);
        }
        */

        // POST api/user/resetpassword
        /// <summary>
        /// Changes old password of the user with the specific ID to the new password.
        /// </summary>
        /// <param name="userId">ID of the user changing password.</param>
        /// <param name="oldPassword">Old password of the user.</param>
        /// <param name="newPassword">New password of the user.</param>
        /// <returns></returns>
        [HttpPut, Route("changePassword")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Password is successfully changed.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no such User.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules. Old and new passwords should match.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the request.")]
        public async Task<bool> ChangePassword([FromBody] ChangePassword user)
        {
            return await _userService.ChangePasswordAsync(user.Id, user.OldPassword, user.NewPassword);
        }

        // POST api/user/resetpassword
        /// <summary>
        /// Asks for the email with a hyperlink which will reset password of the user with this email.
        /// </summary>
        /// <param name="email">Email of the user forgotten its password.</param>
        /// <returns></returns>
        [HttpPut, Route("ResetPassword")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Request is sent")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Email.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the request.")]
        public async Task ResetPassword([FromBody] string email)
        {
            await _userService.ResetPasswordAsync(email);
        }

        // POST api/user/updateemail
        /// <summary>
        /// Updates email of the user to the new email.
        /// </summary>
        /// <param name="user">User model with two fields: string Id and string Email</param>
        /// <returns></returns>
        [HttpPut, Route("UpdateEmail")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Email is updated.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the update.")]
        public async Task<bool> UpdateEmail([FromBody] UpdateEmailORUserName user)
        {
            return await _userService.UpdateEmailAsync(user.Id, user.EmailORUserName);
        }

        //PUT api/user/UpdateUserName
        /// <summary>
        /// Updates user name of the user with the specific ID.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut, Route("UpdateUserName")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "UserName is updated.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the update.")]
        public async Task<bool> UpdateUserName([FromBody] UpdateEmailORUserName user)
        {
            return await _userService.UpdateUserNameAsync(user.Id, user.EmailORUserName);
        }

        // PUT api/User/Friends
        /// <summary>
        /// Adds a new friend to the friendlist of the user with the specific ID.
        /// </summary>
        /// <param name="ids">Ids model contains: string UserId and FriendId</param>
        /// <returns></returns>
        [HttpPut, Route("AddFriend")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Friend is added.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "User already has got the Friend.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the request.")]
        public async Task AddFriend([FromBody] DoubleIdModel ids)
        {
            await _userService.AddFriendAsync(ids.Primary, ids.Secondary);
        }

        //POST api/User/Friends
        /// <summary>
        /// Deletes a friend with the specific friendId ID from the friendlist of the user with the userId ID.
        /// </summary>
        /// <param name="ids">Ids model contains: string UserId and FriendId.</param>
        /// <returns></returns>
        [HttpPost, Route("DeleteFriend")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Friend is removed.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no Friend with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the request.")]
        public async Task DeleteFriend([FromBody] DoubleIdModel ids)
        {
            await _userService.DeleteFriendAsync(ids.Primary, ids.Secondary);
        }

        //PUT api/User/AddProduct
        /// <summary>
        /// Adds product with its rating and description to the user's list of products.
        /// </summary>
        /// <param name="productRate">Product rate contains: string UserId, string ProductId, int Rating, string Description</param>
        /// <returns></returns>
        [HttpPut, Route("AddProduct")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Friend is removed.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User or Product with such Id.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "User already has got the ProductRate.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the request.")]
        public async Task AddProduct([FromBody] ProductRate productRate)
        {
            await _userService.AddProductAsync(productRate.UserId, productRate.ProductId, productRate.Rating, productRate.Description);
        }

        //POST api/User/UpdateProduct
        /// <summary>
        /// Updates Product Description of User.
        /// </summary>
        /// <param name="productRate">Contains: string UserId, string ProductId, int Rating, string Description</param>
        /// <returns></returns>
        [HttpPost, Route("UpdateProduct")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Feedback is modified.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no ProductRate with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please stick to validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the request.")]
        public async Task UpdateProduct([FromBody] ProductRate productRate)
        {
            await _userService.UpdateProductDescriptionAsync(productRate.UserId, productRate.ProductId, productRate.Rating, productRate.Description);
        }

        //POST api/User/DeleteProduct
        /// <summary>
        /// Deletes product from the user's list of products.
        /// </summary>
        /// <param name="ids">Ids model contains: string UserId and ProductRateId</param>
        /// <returns></returns>
        [HttpPost, Route("DeleteProduct")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product is removed.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no ProductRate with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception during the request.")]
        public async Task DeleteProduct([FromBody]DoubleIdModel ids)
        {
            await _userService.DeleteProductAsync(ids.Primary, ids.Secondary);
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