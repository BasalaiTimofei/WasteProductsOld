using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Models.Users
{
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
    }
}