using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using WasteProducts.Logic.Services.UserService;

namespace WasteProducts.Web.Controllers.Api
{
    [RoutePrefix("Users")]
    public class UserController : BaseApiController
    {

        protected UserController(ILogger logger) : base(logger)
        {
        }


    }
}