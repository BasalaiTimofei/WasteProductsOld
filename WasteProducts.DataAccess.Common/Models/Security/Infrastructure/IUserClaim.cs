
namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the IUserClaim.
    /// </summary>
    public interface IUserClaim 
    {

        /// <summary>
        /// User claim Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// Navigation user property
        /// </summary>
        IAppUser User { get; set; }

        /// <summary>
        /// Type of claim
        /// </summary>
        string ClaimType { get; set; }

        /// <summary>
        /// Value of claim
        /// </summary>
        string ClaimValue { get; set; }

    }
}
