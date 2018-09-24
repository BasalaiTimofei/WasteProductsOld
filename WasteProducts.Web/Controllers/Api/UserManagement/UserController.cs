using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users.WebUsers;
using WasteProducts.Logic.Common.Services.Users;
using WasteProducts.Web.ExceptionHandling.Api;
using WasteProducts.Web.ExceptionHandling.Exceptions;
using WasteProducts.Web.Models.Users;
using WasteProducts.Web.Validators.Users;
using FluentValidation;

namespace WasteProducts.Web.Controllers.Api.UserManagement
{
    /// <summary>
    /// API controller for user management.
    /// </summary>
    [RoutePrefix("api/user")]
    [WasteExceptionFilter]
    public class UserController : BaseApiController
    {
        private readonly IUserService _service;

        /// <summary>
        /// Creates an Instance of UserController. User controller links frontend and business logic.
        /// </summary>
        /// <param name="userService">Instance of UserService from business logic.</param>
        /// <param name="logger">Instance of NLog Logger.</param>
        public UserController(IUserService userService, ILogger logger) : base(logger)
        {
            _service = userService;
        }

        //GET api/user
        /// <summary>
        /// Gets all users of the WasteProducts application.
        /// </summary>
        /// <returns>IEnumerable of all the users.</returns>
        [HttpGet, Route("")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Users are found.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There are no Users.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the search.")]
        public async Task<IHttpActionResult> GetUsers()
        {
            var users = await _service.GetAllUsersAsync();

            if (!users.Any())
            {
                throw new WasteException("There are no Users.", HttpStatusCode.NotFound);
            }

            return Ok(users);
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
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such ID.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the search.")]
        public async Task<IHttpActionResult> GetUserById(string id)
        {
            var user = await _service.GetUserAsync(id);
            if (user is null)
            {
                throw new WasteException("There is no User with such ID.", HttpStatusCode.NotFound);
            }
            return Ok(user);
        }

        /// <summary>
        /// Gets user by its email and password.
        /// </summary>
        /// <param name="user">PLL model, contains UserNameOREmail (for this method it would be email), password (Password of the user).</param>
        /// <returns>User with the specific email and password or null if there is no matches.</returns>
        [HttpPost, Route("loginbyemail")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "User was successfully logged in.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Please provide correct Email and Password.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "Already logged in.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the logging in.")]
        public async Task<IHttpActionResult> LoginByEmailAndPassword([FromBody]LoginByEmail user)
        {
            // throws 400
            var validator = new LoginByEmailValidator();
            validator.ValidateAndThrow(user);

            var returnUser = await _service.LogInByEmailAsync(user.Email, user.Password);
            if (returnUser is null)
            {
                throw new WasteException("Please provide correct Email and Password.", HttpStatusCode.Unauthorized);
            }

            return Ok(returnUser);
        }

        /// <summary>
        /// Gets user by its name and password.
        /// </summary>
        /// <param name="user">PLL model, contains UserName (User's name) and password (Password of the user).</param>
        /// <returns>User with the specific email and password or null if there is no matches.</returns>
        [HttpPost, Route("loginbyusername")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "User was successfully logged in.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Please provide correct UserName and Password.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "Already logged in.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the logging in.")]
        public async Task<IHttpActionResult> LoginByNameAndPassword([FromBody]LoginByName user)
        {
            //throws 400
            var validator = new LoginByNameValidator();
            validator.ValidateAndThrow(user);

            var returnedUser = await _service.LogInByNameAsync(user.UserName, user.Password);

            if (returnedUser is null)
            {
                throw new WasteException("Please provide correct User Name and Password.", HttpStatusCode.Unauthorized);
            }
            return Ok(user);
        }

        /// <summary>
        /// Gets all user's friends.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <returns></returns>
        [HttpGet, Route("{id}/friends")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Friends of the user returned.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<IHttpActionResult> GetFriends([FromUri] string id)
        {
            return Ok(await _service.GetFriendsAsync(id));
        }

        /// <summary>
        /// Gets all user's products.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <returns></returns>
        [HttpGet, Route("{id}/products")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Products of the user returned.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<IHttpActionResult> GetProductDescriptions([FromUri] string id)
        {
            return Ok(await _service.GetProductDescriptionsAsync(id));
        }

        /// <summary>
        /// Gets all user's groups.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <returns></returns>
        [HttpGet, Route("{id}/groups")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Groups of the user returned.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<IHttpActionResult> GetGroups([FromUri] string id)
        {
            return Ok(await _service.GetGroupsAsync(id));
        }

        /// <summary>
        /// Gets all the roles of the user with the specific ID.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <returns>IList of roles of the user.</returns>
        [HttpGet, Route("{id}/roles")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Roles of the user returned.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<IList<string>> GetRoles(string id)
        {
            return await _service.GetRolesAsync(id);
        }

        /// <summary>
        /// Gets all claims of a user with the specific ID.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <returns>IList of claims of the user.</returns>
        [HttpGet, Route("{id}/claims")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Claims of the user returned.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<IList<Claim>> GetClaims(string id)
        {
            return await _service.GetClaimsAsync(id);
        }

        /// <summary>
        /// Gets all logins of a user with the specific ID.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <returns>IList of logins of the user.</returns>
        [HttpGet, Route("{id}/logins")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Logins of the user returned.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<IList<UserLogin>> GetLogins(string id)
        {
            return await _service.GetLoginsAsync(id);
        }

        /// <summary>
        /// Registers a new user with the specific email, name and password.
        /// </summary>
        /// <param name="model">User model that contains: Email, UserName and Password.</param>
        /// <returns></returns>
        [HttpPost, Route("register")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "User was successfully registered.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "Please provide unique UserName and Email.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the registration.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        public async Task<IHttpActionResult> Register([FromBody] RegisterUser model)
        {
            var sb = new StringBuilder(Request.RequestUri.GetLeftPart(UriPartial.Authority));
            sb.Append("/api/user/{0}/confirmemail/{1}");

            //throws 400
            var validator = new RegisterUserValidator();
            validator.ValidateAndThrow(model);

            var idToken = await _service.RegisterAsync(model.Email, model.UserName, model.Password, sb.ToString());

            if (idToken.id is null && idToken.token is null)
            {
                throw new WasteException("Please provide unique UserName and Email.", HttpStatusCode.Conflict);
            }
            return Ok(idToken);
        }

        /// <summary>
        /// Deletes user from the application.
        /// </summary>
        /// <param name="id">Id of the deleting user.</param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "User is deleted.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the deletion.")]
        public async Task<IHttpActionResult> Delete([FromUri] string id)
        {
            await _service.DeleteUserAsync(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Confirms user's email by the confirmation token.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <param name="token">Confirmation token.</param>
        /// <returns>Boolean represents whether operation succeed or no.</returns>
        [HttpGet, Route("{id}/confirmemail/{token}")]
        public async Task<bool> ConfirmEmail([FromUri] string id, [FromUri] string token)
        {
            return await _service.ConfirmEmailAsync(id, token);
        }

        /// <summary>
        /// Changes old password of the user with the specific ID to the new password.
        /// </summary>
        /// <param name="id">ID of the user changing its password.</param>
        /// <param name="model">PLL model, contains OldPassword (old password of the user) and NewPassword (new password of the user).</param>
        /// <returns></returns>
        [HttpPut, Route("{id}/changepassword")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Password is successfully changed.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no such User.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<IHttpActionResult> ChangePassword([FromUri] string id, [FromBody] ChangePassword model)
        {
            // throws 400
            var validator = new ChangePasswordValidator();
            validator.ValidateAndThrow(model);

            // throws 404
            var user = await this.GetUserById(id);

            var isChanged = await _service.ChangePasswordAsync(id, model.OldPassword, model.NewPassword);

            return Ok(isChanged);
        }

        /// <summary>
        /// Requests for the email with a hyperlink which will reset password of the user with this email.
        /// </summary>
        /// <param name="email">Email of the user forgotten its password.</param>
        /// <returns></returns>
        [HttpPut, Route("resetpasswordrequest")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Request is sent")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<IHttpActionResult> ResetPasswordRequest([FromBody] Email email)
        {
            //throws 400
            var validator = new EmailValidator();
            validator.ValidateAndThrow(email);

            var sb = new StringBuilder(Request.RequestUri.GetLeftPart(UriPartial.Authority));
            sb.Append("/api/user/{0}/resetpasswordresponse/{1}");
            await _service.ResetPasswordRequestAsync(email.EmailOfTheUser, sb.ToString());

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Resets a user's password to the new password by confirmation token.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <param name="token">Reset password token, sended to the user's email.</param>
        /// <param name="newPassword"></param>
        /// <returns>Boolean represents whether operation succeed or no.</returns>
        [HttpPut, Route("{id}/resetpasswordresponse/{token}")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Password is changed.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Invalid token or id.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<IHttpActionResult> ResetPasswordResponse([FromUri] string id, [FromUri] string token, [FromBody] NewPassword newPassword)
        {
            //throws 400
            var validator = new NewPasswordValidator();
            validator.ValidateAndThrow(newPassword);

            var isSucceed = await _service.ResetPasswordAsync(id, token, newPassword.Password);


            if (isSucceed)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return StatusCode(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Updates email of the user to the new email.
        /// </summary>
        /// <param name="id">ID of the user changing its email</param>
        /// <param name="newEmail">New email of the user.</param>
        /// <returns></returns>
        [HttpPut, Route("{id}/updateemail")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Email is updated.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the update.")]
        public async Task<bool> UpdateEmail([FromUri] string id, [FromBody] string newEmail)
        {
            return await _service.UpdateEmailAsync(id, newEmail);
        }

        /// <summary>
        /// Updates user name of the user with the specific ID.
        /// </summary>
        /// <param name="id">ID of the user changing its userName.</param>
        /// <param name="newUserName">New name of the user.</param>
        /// <returns></returns>
        [HttpPut, Route("{id}/updateusername")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "UserName is updated.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please follow the validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the update.")]
        public async Task<bool> UpdateUserName([FromUri] string id, [FromBody] string newUserName)
        {
            return await _service.UpdateUserNameAsync(id, newUserName);
        }

        /// <summary>
        /// Adds a new friend to the friendlist of the user with the specific ID.
        /// </summary>
        /// <param name="userId">ID of the user adding new friend.</param>
        /// <param name="friendId">ID of the new friend of the user.</param>
        /// <returns></returns>
        [HttpPut, Route("{userId}/addfriend/{friendId}")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Friend is added.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User with such Id.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "User already has got the Friend.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task AddFriend([FromUri] string userId, [FromUri] string friendId)
        {
            await _service.AddFriendAsync(userId, friendId);
        }

        /// <summary>
        /// Deletes a friend with the specific friendId ID from the friendlist of the user with the userId ID.
        /// </summary>
        /// <param name="userId">ID of the user deleting friend.</param>
        /// <param name="friendId">ID of the deleting friend.</param>
        /// <returns></returns>
        [HttpPut, Route("{userId}/deletefriend/{friendId}")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Friend is removed.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no Friend with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task DeleteFriend([FromUri] string userId, [FromUri] string friendId)
        {
            await _service.DeleteFriendAsync(userId, friendId);
        }

        /// <summary>
        /// Adds product with its rating and description to the user's list of products.
        /// </summary>
        /// <param name="userId">ID of the user adding the product.</param>
        /// <param name="productId">ID of the adding product.</param>
        /// <param name="description">PLL model contains Rating and Description</param>
        /// <returns></returns>
        [HttpPut, Route("{userId}/addproduct/{productId}")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Product added and described.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no User or Product with such Id.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "User already has got the ProductRate.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task AddProduct([FromUri] string userId, [FromUri] string productId, [FromBody] ProductDescriprion description)
        {
            await _service.AddProductAsync(userId, productId, description.Rating, description.Description);
        }

        /// <summary>
        /// Updates Product Description of User.
        /// </summary>
        /// <param name="userId">ID of the user adding the product.</param>
        /// <param name="productId">ID of the adding product.</param>
        /// <param name="description">PLL model contains Rating and Description</param>
        /// <returns></returns>
        [HttpPut, Route("{userId}/updateproductdescription/{productId}")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Feedback is modified.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no ProductRate with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Please stick to validation rules.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task UpdateProduct([FromUri] string userId, [FromUri] string productId, [FromBody] ProductDescriprion description)
        {
            await _service.UpdateProductDescriptionAsync(userId, productId, description.Rating, description.Description);
        }

        /// <summary>
        /// Deletes product from the user's list of products.
        /// </summary>
        /// <param name="userId">ID of the user who is adding a product to its list of products.</param>
        /// <param name="productId">ID of adding product.</param>
        /// <returns></returns>
        [HttpPut, Route("{userId}/deleteproduct/{productId}")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Product is removed.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no ProductRate with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task DeleteProduct([FromUri] string userId, [FromUri] string productId)
        {
            await _service.DeleteProductAsync(userId, productId);
        }

        /// <summary>
        /// Confirms group invitation if isConfirmed == true or deletes invite if isConfirmed == false.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="groupId">ID of the group.</param>
        /// <param name="isConfirmed">True if invitation accepted or false if not.</param>
        /// <returns></returns>
        [HttpPut, Route("{userId}/respondtogroupinvitation/{groupId}")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Respond given back.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no user or group with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<HttpStatusCode> RespondToGroupInvitation([FromUri] string userId, [FromUri] string groupId, [FromBody] bool isConfirmed)
        {
            await _service.RespondToGroupInvitationAsync(userId, groupId, isConfirmed);
            return HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Leave from group by the user. 
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="groupId">ID of the group.</param>
        /// <returns></returns>
        [HttpPut, Route("{userId}/leavegroup/{groupId}")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Group have been left.")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There is no user or group with such Id.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You don't have enough permissions.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        public async Task<HttpStatusCode> LeaveGroup([FromUri] string userId, [FromUri] string groupId)
        {
            await _service.LeaveGroupAsync(userId, groupId);
            return HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Adds a user with the specific ID to the role.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        [HttpPut, Route("{id}/addtorole/{roleName}")]
        public async Task AddToRole([FromUri] string id, [FromUri]string roleName)
        {
            await _service.AddToRoleAsync(id, roleName);
        }

        /// <summary>
        /// Removes a user with the specific ID from the role.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        [HttpPut, Route("{id}/removefromrole/{roleName}")]
        public async Task RemoveFromRole([FromUri] string id, [FromUri]string roleName)
        {
            await _service.RemoveFromRoleAsync(id, roleName);
        }

        /// <summary>
        /// Adds a claim to the user.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <param name="claim">Claim to add to the user.</param>
        /// <returns></returns>
        [HttpPut, Route("{id}/addclaim")]
        public async Task AddClaim([FromUri] string id, [FromBody]Claim claim)
        {
            await _service.AddClaimAsync(id, claim);
        }

        /// <summary>
        /// Removes a claim from the user.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <param name="claim">Claim to remove from the user.</param>
        /// <returns></returns>
        [HttpPut, Route("{id}/removeclaim")]
        public async Task RemoveClaim([FromUri] string id, [FromBody]Claim claim)
        {
            await _service.RemoveClaimAsync(id, claim);
        }

        /// <summary>
        /// Adds a login to the user.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <param name="userLogin">Login to add to the user.</param>
        /// <returns></returns>
        [HttpPut, Route("{id}/addlogin")]
        public async Task AddLogin([FromBody] string id, [FromBody]UserLogin userLogin)
        {
            await _service.AddLoginAsync(id, userLogin);
        }

        /// <summary>
        /// Removes a login to the user.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <param name="userLogin">Login to remove from the user.</param>
        /// <returns></returns>
        [HttpPut, Route("{id}/removelogin")]
        public async Task RemoveLogin([FromBody] string id, [FromBody]UserLogin userLogin)
        {
            await _service.RemoveLoginAsync(id, userLogin);
        }
    }
}