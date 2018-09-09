using System;
using System.Collections.Generic;
using System.Security.Claims;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Models.Users
{
    // Удалим лишние проперти когда разберемся, а какие из них вообще лишние на бизнес-слое
    /// <summary>
    /// Full BL version of UserDB.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique key for the user.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Unique username.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// List of Users which belong to group of friends related to current User.
        /// </summary>
        public virtual IList<User> Friends { get; set; }

        /// <summary>
        /// List of products added and described by the user.
        /// </summary>
        public virtual IList<UserProductDescription> ProductDescriptions { get; set; }

        // TODO decomment after the "Groups" model is enabled
        /// <summary>
        /// List of all Groups to which current User is assigned.
        /// </summary>
        //public virtual List<Group> GroupMembership { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// PhoneNumber for the user.
        /// </summary>
        public virtual string PhoneNumber { get; set; }
    }
}