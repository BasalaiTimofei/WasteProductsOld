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
        //todo async
        public List<User> Get()
        {
            return _userService.GetAllUsersInfo();
        }

        // GET api/user/5
        [Route("{id:string}")]
        public async Task<User> Get(string id)
        {
            //todo validation
            return await _userService.GetUserInfo(id);
        }

        // PUT api/user/5
        [Route("{id:string}")]
        [HttpPut]
        public async Task Update(string id, [FromBody]User user)
        {
            await _userService.UpdateAsync(user);
        }

        // POST api/user
        [HttpPost]
        [Route("api/User/Login")]
        public async Task<User> LogIn(string id, string password)
        {
            return await _userService.LogInAsync(id, password);
        }

        // DELETE api/user/5
        //todo can we user controller methods in controller methods?
        [Route("{id:string}")]
        [HttpDelete]
        public async Task Delete(string userId)
        {
            await _userService.DeleteUserAsync(userId);
        }



        // POST api/User/Register
        [Route("api/User/Register")]
        [HttpPost]
        public async Task<User> Register([FromBody] string email, [FromBody] string userName, [FromBody] string password)
        {
            //todo get from JSON, not User instance
            return await _userService.RegisterAsync(email, userName, password);
        }



        // POST api/user/resetpassword
        [Route("api/User/ResetPassword")]
        [HttpPut]
        public async Task<bool> ResetPassword([FromBody]User user, string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            return await _userService.ResetPasswordAsync(user, oldPassword, newPassword, newPasswordConfirmation);
        }

        // POST api/user/resetpassword
        [Route("api/User/ResetPassword")]
        [HttpPut]
        public async Task ResetPassword(string email)
        {
            await _userService.ResetPasswordAsync(email);
        }



        // POST api/user/updateemail
        [Route("api/User/UpdateEmail")]
        [HttpPost]
        public async Task<bool> UpdateEmail([FromBody]User user, [FromBody]string newEmail)
        {
            return await _userService.UpdateEmailAsync(user.Id, newEmail);
        }



        //POST api/user/UpdateUserName
        [Route("api/User/UpdateUserName")]
        [HttpPost]
        public async Task<bool> UpdateUserName([FromBody]User user, [FromBody]string newUserName)
        {
            return await _userService.UpdateUserNameAsync(user, newUserName);
        }



        // PUT api/User/Friends
        [Route("api/User/Friends")]
        [HttpPut]
        public async Task AddFriend([FromBody]User user, [FromBody]User newFriend)
        {
            await _userService.AddFriendAsync(user, newFriend);
        }

        //POST api/User/Friends
        [Route("api/User/Friends/delete")]
        [HttpPost]
        public async Task DeleteFriend([FromBody]User user, [FromBody]User friend)
        {
            await _userService.DeleteFriendAsync(user, friend);
        }



        //PUT api/User/Products
        [Route("api/User/Product")]
        [HttpPut]
        public async Task AddProduct([FromBody]string userId, [FromBody]string productId, [FromBody]int rating, [FromBody]string description)
        {
            await _userService.AddProductAsync(userId, productId, rating, description);
        }

        //POST api/User/Products/Delete
        [Route("api/User/Products/Delete")]
        public async Task DeleteProduct([FromBody]string userId, [FromBody]string productId)
        {
            await _userService.DeleteProductAsync(userId, productId);
        }



        // POST api/User/Roles
        [Route("api/User/Roles")]
        [HttpPost]
        public async Task<IList<string>> GetRoles([FromBody]User user)
        {
            return await _userService.GetRolesAsync(user);
        }

        // DELETE api/User/Roles
        [Route("api/User/Roles")]
        public async Task RemoveFromRole([FromBody] string userId, [FromBody]string role)
        {
            await _userService.RemoveFromRoleAsync(userId, role);
        }

        // PUT api/User/Roles
        [Route("api/User/Roles")]
        [HttpPut]
        public async Task AddToRole([FromBody] string userId, [FromBody]string roleName)
        {
            await _userService.AddToRoleAsync(userId, roleName);
        }



        //DELETE api/User/Claims
        [Route("api/User/Claims")]
        [HttpDelete]
        public async Task RemoveClaim([FromBody] string userId, [FromBody]Claim claim)
        {
            await _userService.RemoveClaimAsync(userId, claim);
        }

        // PUT api/User/Claims
        [Route("api/User/Claims")]
        [HttpPut]
        public async Task AddClaim([FromBody] string userId, [FromBody]Claim claim)
        {
            await _userService.AddClaimAsync(userId, claim);
        }



        // PUT api/User/Logins
        [Route("api/User/Logins")]
        public async Task AddLogin([FromBody] string userId, [FromBody]UserLogin userLogin)
        {
            await _userService.AddLoginAsync(userId, userLogin);
        }

        //DELETE api/User/Logins
        [Route("api/User/Logins")]
        [HttpDelete]
        public async Task RemoveLogin([FromBody] string userId, [FromBody]UserLogin userLogin)
        {
            await _userService.RemoveLoginAsync(userId, userLogin);
        }
    }
}