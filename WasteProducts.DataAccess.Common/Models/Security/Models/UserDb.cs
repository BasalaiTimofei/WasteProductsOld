using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    /// <summary>
    /// Class UserDb. Has an inheritance from IUserDb. Security model class
    /// </summary>
    public class UserDb : IUserDb
    {

        #region Fields
        /// <summary>
        /// field for Claim navigation property
        /// </summary>
        private ICollection<IClaimDb> _claims;

        /// <summary>
        /// field for UserLogin navigation property
        /// </summary>
        private ICollection<IUserLoginDb> _externalLogins;

        /// <summary>
        /// field for Role navigation property
        /// </summary>
        private ICollection<IRoleDb> _roles;
        #endregion

        #region Scalar Properties
        /// <summary>
        /// User id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User pasword hash value
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// User Security Stamp
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User status of email was Confirm or not
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// User PhoneNumber
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User status of Phone Number was Confirm or not
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// user ability to authentification (sms+email)
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the date time value (in UTC) when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether lockout enabled for this user.
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Count of failed access
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Date when was be created User
        /// </summary>
        public DateTime CreateDate { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// Claim navigation property
        /// </summary>
        public virtual ICollection<IClaimDb> Claims
        {
            get { return _claims ?? (_claims = new List<IClaimDb>()); }
            set { _claims = value; }
        }

        /// <summary>
        /// UserLogin navigation property
        /// </summary>
        public virtual ICollection<IUserLoginDb> Logins
        {
            get
            {
                return _externalLogins ??
                    (_externalLogins = new List<IUserLoginDb>());
            }
            set { _externalLogins = value; }
        }

        /// <summary>
        /// Role navigation property
        /// </summary>
        public virtual ICollection<IRoleDb> Roles
        {
            get { return _roles ?? (_roles = new List<IRoleDb>()); }
            set { _roles = value; }
        }
        #endregion
    }
}
