namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    /// <summary>
    /// Interface for the UserLoginDb class.
    /// </summary>
    public interface IUserLoginDb
    {
        /// <summary>
        /// Login Provider
        /// </summary>
        string LoginProvider { get; set; }

        /// <summary>
        /// Provider Key
        /// </summary>
        string ProviderKey { get; set; }

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