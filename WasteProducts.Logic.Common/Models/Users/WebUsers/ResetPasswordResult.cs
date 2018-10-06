using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WasteProducts.Logic.Common.Models.Users.WebUsers
{
    /// <summary>
    /// Represents result of ResetPassword method.
    /// </summary>
    public class ResetPasswordResult
    {
        /// <summary>
        /// ID of the user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Reset password token.
        /// </summary>
        public string Token { get; set; }
    }
}