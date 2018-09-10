using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WasteProducts.Web.Models.Users
{
    public class UpdateEmailORUserName
    {
        public string UserId { get; set; }

        public string EmailORUserName { get; set; }
    }
}