using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;
namespace WasteProducts.DataAccess.Common.Models.Security.Models

{
    /// <summary>
    /// Class ClaimDb. Has an inheritance from IClaimDb. Security model class
    /// </summary>
    public class ClaimDb : IClaimDb
    {
        /// <summary>
        /// user field
        /// </summary>
        private IUserDb _user;

        #region Scalar Properties
        /// <summary>
        /// User claim Id
        /// </summary>
        public virtual int ClaimId { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// Type of claim
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        /// Value of claim
        /// </summary>
        public virtual string ClaimValue { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// Navigation user property
        /// </summary>
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
