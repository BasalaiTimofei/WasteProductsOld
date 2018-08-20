using Microsoft.AspNet.Identity;
using System;

namespace WasteProducts.Logic.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the User. Has an inheritance from Microsoft.AspNet.Identity.IUser.
    /// </summary>
    public interface IAppUser : IUser<int>
    {
        /// <summary>
        /// User email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// User status of email was Confirm or not
        /// </summary>
        bool EmailConfirmed { get; set; }

        /// <summary>
        /// User status of lock
        /// </summary>
        bool LockoutEnabled { get; set;}

        /// <summary>
        /// DateTime when user was be blocked
        /// </summary>
        DateTime? LockoutEnabledUtc { get; set; }

        /// <summary>
        /// Count of failde access
        /// </summary>
        int AccessFailedCount { get; set; }

        /// <summary>
        /// User pasword hash value
        /// </summary>
        string PasswordHash { get; set; }

        /// <summary>
        /// User Security Stamp
        /// </summary>
        string SecurityStamp { get; set; }

        /// <summary>
        /// User PhoneNumber
        /// </summary>
        string PhoneNumber { get; set; }

        /// <summary>
        /// User status of Phone Number was Confirm or not
        /// </summary>
        bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// user ability to authentification (sms+email)
        /// </summary>
        bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Date when was be created User
        /// </summary>
        DateTime CreateDate { get; set; }

    }
}
