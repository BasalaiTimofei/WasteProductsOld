using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    /// <summary>
    /// Class UserRole. Has an inheritance from IUserRole. Security model class
    /// </summary>
    public class UserRoleDb : IUserRoleDb
    {
        /// <summary>
        /// Field for User navigation property
        /// </summary>
        private IUserDb _user;

        /// <summary>
        /// Field for User navigation property
        /// </summary>
        private IRoleDb _role;

        #region Scalar Properties
        /// <summary>
        /// User Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// Role Id
        /// </summary>
        public virtual int RoleId { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// Navigation Role property
        /// </summary>
        public virtual IRoleDb Role
        {
            get { return _role; }
            set
            {
                _role = value;
                RoleId = value.Id;
            }
        }

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
