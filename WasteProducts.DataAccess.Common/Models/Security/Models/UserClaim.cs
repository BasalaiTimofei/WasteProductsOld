using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    class UserClaim : IUserClaim
    {
        /// <summary>
        /// User claim Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation user property
        /// </summary>
        public IAppUser User { get; set; }

        /// <summary>
        /// Type of claim
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// Value of claim
        /// </summary>
        public string ClaimValue { get; set; }
    }
}
