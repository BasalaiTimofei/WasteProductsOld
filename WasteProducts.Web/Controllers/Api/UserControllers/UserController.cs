using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace WasteProducts.Web.Controllers.Api
{
    public class UserController : BaseApiController
    {
        public UserController(ILogger logger) : base(logger)
        {
        }
    }
}