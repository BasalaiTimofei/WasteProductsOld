using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WasteProducts.Web.Models.Users
{
    /// <summary>
    /// PLL model for logging in to the server.
    /// </summary>
    public class LoginByEmail
    {
        /// <summary>
        /// Name of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}