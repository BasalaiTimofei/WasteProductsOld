using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models

{
    public class ClaimDb : IClaimDb
    {
        private IUserDb _user;

        #region Scalar Properties
        public virtual int ClaimId { get; set; }
        public virtual int UserId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
        #endregion

        #region Navigation Properties
        public virtual IUserDb User
        {
            get { return _user; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _user = value;
                UserId = value.Id;
            }
        }
        #endregion

    }
}
