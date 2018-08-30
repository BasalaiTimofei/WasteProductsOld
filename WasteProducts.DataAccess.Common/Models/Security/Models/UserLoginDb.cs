using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    /// <summary>
    /// Class UserLoginDb. Has an inheritance from IUserLoginDb. Security model class
    /// </summary>
    public class UserLoginDb : IUserLoginDb
    {
        /// <summary>
        /// Field for user navigation property
        /// </summary>
        private IUserDb _user;

        #region Scalar Properties
        /// <summary>
        /// Login Provider
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Provider Key
        /// </summary>
        public virtual string ProviderKey { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public virtual int UserId { get; set; }
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
                _user = value;
                UserId = value.Id;
            }
        }
        #endregion
    }
}
