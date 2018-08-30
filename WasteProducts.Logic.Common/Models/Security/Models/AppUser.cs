using System;
using WasteProducts.Logic.Common.Models.Security.Infrastructure;

namespace WasteProducts.Logic.Common.Models.Security.Models
{
    public class AppUser : IAppUser
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User status of email was Confirm or not
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// User status of lock
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// DateTime when user was be blocked
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Count of failde access
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        ///  User pasword hash value
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// user security stamp
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// user phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  User status of Phone Number was Confirm or not
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// user ability to authentification (sms+email)
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Date when was be created user
        /// </summary>
        public DateTime CreateDate { get; set; }

    }
}
