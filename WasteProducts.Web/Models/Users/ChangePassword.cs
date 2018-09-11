using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WasteProducts.Web.Models.Users
{
    /// <summary>
    /// PLL model for changing password.
    /// </summary>
    public class ChangePassword
    {
        /// <summary>
        /// Old password of the user.
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// New password of the user.
        /// </summary>
        public string NewPassword { get; set; }
    }
}