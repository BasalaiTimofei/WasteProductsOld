using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    /// <summary>
    /// Data access level model of user.
    /// </summary>
    public class UserDAL
    {
        /// <summary>
        /// Unique key for the user
        /// </summary>
        public virtual string Id { get; set ; }

        /// <summary>
        /// Unique username
        /// </summary>
        public virtual string UserName { get ; set ; }

        /// <summary>
        /// List of Users which belong to group of friends related to current User
        /// </summary>
        public virtual IList<UserDB> Friends { get; set; }

        /// <summary>
        /// List of products added and described by the user
        /// </summary>
        public virtual IList<UserProductDescriptionDB> ProductDescriptions { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// True if the email is confirmed, default is false
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// PhoneNumber for the user
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// True if the phone number is confirmed, default is false
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Is two factor enabled for the user
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Is lockout enabled for this user
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Used to record failures for the purposes of lockout
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Navigation property for user roles
        /// </summary>
        public virtual ICollection<IdentityUserRole> Roles { get; set; }

        /// <summary>
        /// Navigation property for user claims
        /// </summary>
        public virtual ICollection<IdentityUserClaim> Claims { get; set; }

        /// <summary>
        /// Navigation property for user logins
        /// </summary>
        public virtual ICollection<IdentityUserLogin> Logins { get; set; }

        /// <summary>
        /// Specifies timestamp of creation of concrete User in Database.
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Specifies timestamp of modifying of any Property of User in Database.
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}
