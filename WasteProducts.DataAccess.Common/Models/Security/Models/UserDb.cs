using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    public class UserDb : IUserDb
    {

        #region Fields
        private ICollection<IClaimDb> _claims;
        private ICollection<IUserLoginDb> _externalLogins;
        private ICollection<IRoleDb> _roles;
        #endregion

        #region Scalar Properties
        public int UserId { get; set; }
        public string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public DateTime CreateDate { get; set; }
        #endregion

        #region Navigation Properties
        public virtual ICollection<IClaimDb> Claims
        {
            get { return _claims ?? (_claims = new List<IClaimDb>()); }
            set { _claims = value; }
        }

        public virtual ICollection<IUserLoginDb> Logins
        {
            get
            {
                return _externalLogins ??
                    (_externalLogins = new List<IUserLoginDb>());
            }
            set { _externalLogins = value; }
        }

        public virtual ICollection<IRoleDb> Roles
        {
            get { return _roles ?? (_roles = new List<IRoleDb>()); }
            set { _roles = value; }
        }
        #endregion
    }
}
