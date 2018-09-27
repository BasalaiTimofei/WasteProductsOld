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
        public string Id { get; set; }

        /// <summary>
        /// Unique username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// True if email was confirmed by token.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// True if phone number was confirmed by token.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }
    }
}