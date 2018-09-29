using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WasteProducts.Web.Models.Users
{
    /// <summary>
    /// This model contains only one Property with new Password.
    /// </summary>
    public class NewPassword
    {
        /// <summary>
        /// New password that will be set to the User.
        /// </summary>
        public string Password { get; set; }
    }
}