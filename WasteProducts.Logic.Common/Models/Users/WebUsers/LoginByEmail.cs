using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WasteProducts.Logic.Common.Models.Users.WebUsers
{
    /// <summary>
    /// PLL model for logging in to the server.
    /// </summary>
    public class LoginByEmail
    {
        /// <summary>
        /// Name of the user.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Password of the user.
        /// </summary>
        /// 
        [Required]
        public string Password { get; set; }
    }
}