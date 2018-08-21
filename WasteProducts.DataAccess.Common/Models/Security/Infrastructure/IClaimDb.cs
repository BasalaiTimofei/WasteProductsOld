namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the ClaimDb.
    /// </summary>
    public interface IClaimDb
    {
        /// <summary>
        /// User claim Id
        /// </summary>
        int ClaimId { get; set; }

        /// <summary>
        /// Type of claim
        /// </summary>
        string ClaimType { get; set; }

        /// <summary>
        /// Value of claim
        /// </summary>
        string ClaimValue { get; set; }

        /// <summary>
        /// Navigation user property
        /// </summary>
        IUserDb User { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        int UserId { get; set; }
    }
}