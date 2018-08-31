using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace WasteProducts.Web.Controllers.Mvc.UserControllers
{
    public class UserController : BaseMvcController
    {
        protected UserController(ILogger logger) : base(logger)
        {
        }
    }
}