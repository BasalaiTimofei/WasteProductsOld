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
        public async Task Delete(string id)
        {
            var user = await Get(id);
            await _userService.DeleteUserAsync(user);
        }



        // POST api/User/Register
        [Route("api/User/Register")]
        [HttpPost]
        public async Task<User> Register([FromBody]User value)
        {
            //todo get from JSON, not User instance
            return await _userService.RegisterAsync(value.Email, value.UserName, value.PasswordHash, value.PasswordHash);
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
        public async Task AddProduct([FromBody]User user, [FromBody]Product product)
        {
            await _userService.AddProductAsync(user, product);
        }

        //POST api/User/Products/Delete
        [Route("api/User/Products/Delete")]
        public async Task DeleteProduct([FromBody]User user, [FromBody]Product product)
        {
            await _userService.DeleteProductAsync(user, product);
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
        public async Task RemoveFromRole([FromBody]User user, [FromBody]string role)
        {
            await _userService.RemoveFromRoleAsync(user, role);
        }

        // PUT api/User/Roles
        [Route("api/User/Roles")]
        [HttpPut]
        public async Task AddToRole([FromBody]User user, [FromBody]string roleName)
        {
            await _userService.AddToRoleAsync(user, roleName);
        }



        //DELETE api/User/Claims
        [Route("api/User/Claims")]
        [HttpDelete]
        public async Task RemoveClaim([FromBody]User user, [FromBody]Claim claim)
        {
            await _userService.RemoveClaimAsync(user, claim);
        }

        // PUT api/User/Claims
        [Route("api/User/Claims")]
        [HttpPut]
        public async Task AddClaim([FromBody]User user, [FromBody]Claim claim)
        {
            await _userService.AddClaimAsync(user, claim);
        }



        // PUT api/User/Logins
        [Route("api/User/Logins")]
        public async Task AddLogin([FromBody]User user, [FromBody]UserLogin userLogin)
        {
            await _userService.AddLoginAsync(user, userLogin);
        }

        //DELETE api/User/Logins
        [Route("api/User/Logins")]
        [HttpDelete]
        public async Task RemoveLogin([FromBody]User user, [FromBody]UserLogin userLogin)
        {
            await _userService.RemoveLoginAsync(user, userLogin);
        }
    }
}