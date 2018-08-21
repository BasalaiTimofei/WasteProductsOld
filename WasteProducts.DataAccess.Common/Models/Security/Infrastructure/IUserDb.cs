using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the IUserDb. Has an inheritance from Microsoft.AspNet.Identity.IUser.
    /// </summary>
    public interface IUserDb : IUser<int>
    {
        /// <summary>
        /// Count of failed access
        /// </summary>
        int AccessFailedCount { get; set; }

        /// <summary>
        /// Claim navigation property
        /// </summary>
        ICollection<IClaimDb> Claims { get; set; }

        /// <summary>
        /// Date when was be created User
        /// </summary>
        DateTime CreateDate { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// User status of email was Confirm or not
        /// </summary>
        bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether lockout enabled for this user.
        /// </summary>
        bool LockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the date time value (in UTC) when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        DateTime? LockoutEndDateUtc { get; set; }


        /// <summary>
        /// UserLogin navigation property
        /// </summary>
        ICollection<IUserLoginDb> Logins { get; set; }

        /// <summary>
        /// User pasword hash value
        /// </summary>
        string PasswordHash { get; set; }

        /// <summary>
        /// User PhoneNumber
        /// </summary>
        string PhoneNumber { get; set; }

        /// <summary>
        /// User status of Phone Number was Confirm or not
        /// </summary>
        bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Role navigation property
        /// </summary>
        ICollection<IRoleDb> Roles { get; set; }

        /// <summary>
        /// User Security Stamp
        /// </summary>
        string SecurityStamp { get; set; }

        /// <summary>
        /// user ability to authentification (sms+email)
        /// </summary>
        bool TwoFactorEnabled { get; set; }
    }
}