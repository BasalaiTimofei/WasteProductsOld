using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NLog;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.UserService;

namespace WasteProducts.Web.Controllers.Api
{
    [RoutePrefix("UserService")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(ILogger logger) : base(logger)
        {
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
            var user = await _userService.GetUserInfo(id);
            _users.Add(user);
            return _users;
        }

        //todo get (login) password - login ?login=user1?



        // POST api/user
        public async Task<User> Post([FromBody]User value)
        {
            //todo get from JSON, not User instance
            return await _userService.RegisterAsync(value.Email, value.UserName, value.PasswordHash, value.PasswordHash);
        }

        // PUT api/user/5
        [Route("{id:string}")]
        public async Task Put(string id, bool lockoutEnabled, DateTime lockoutEndDateUtc)
        {
            var user = await _userService.GetUserInfo(id);

            user.LockoutEnabled = lockoutEnabled;
            user.LockoutEndDateUtc = lockoutEndDateUtc;

            await _userService.UpdateAsync(user);
        }

        public async Task Put(string id, string password)
        {
            var user = await _userService.GetUserInfo(id);


        }

        // DELETE api/user/5
        //todo can we user controller methods in com=ntroller methods?
        [Route("{id:string}")]
        public async Task Delete(string id)
        {
            var user = Get(id);
            await _userService.DeleteUserAsync(user.GetAwaiter().GetResult());
        }

    }
}