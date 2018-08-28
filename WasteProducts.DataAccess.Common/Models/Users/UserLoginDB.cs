namespace WasteProducts.DataAccess.Common.Models.Users
{
    /// <summary>
    /// Represents a linked login for a user (i.e. a facebook/google account).
    /// </summary>
    public class UserLoginDB
    {
        /// <summary>
        /// Provider for the linked login, i.e. Facebook, Google, etc.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// User specific key for the login provider.
        /// </summary>
        public string ProviderKey { get; set; }
    }
}
