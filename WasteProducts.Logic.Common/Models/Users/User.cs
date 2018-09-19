using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Models.Users
{
    // Удалим лишние проперти когда разберемся, а какие из них вообще лишние на бизнес-слое
    /// <summary>
    /// Standart BLL level version of UserDB.
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
        /// Email of the user.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// True if email was confirmed by token.
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// True if phone number was confirmed by token.
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// List of Users which belong to group of friends related to current User.
        /// </summary>
        //public virtual IList<User> Friends { get; set; }

        /// <summary>
        /// List of products added and described by the user.
        /// </summary>
        //public virtual IList<UserProductDescription> ProductDescriptions { get; set; }
    }
}