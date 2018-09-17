using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WasteProducts.Web.Models.Users
{
    /// <summary>
    /// PLL model for registering a new user.
    /// </summary>
    public class RegisterUser
    {
        /// <summary>
        /// Unique username of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Unique email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}