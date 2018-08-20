using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    /// <summary>
    /// Class UserRole. Has an inheritance from IUserRole. Security model class
    /// </summary>
    public class UserRole : IUserRole
    {
        /// <summary>
        /// User Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation user property
        /// </summary>
        public IAppUser User { get; set; }

        /// <summary>
        /// Role Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Navigation Role property
        /// </summary>
        public IAppRole Role { get; set; }
    }
}
