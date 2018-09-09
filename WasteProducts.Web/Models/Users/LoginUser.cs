using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WasteProducts.Web.Models.Users
{
    public class LoginUser
    {
        public string UserNameOREmail { get; set; }

        public string Password { get; set; }
    }
}