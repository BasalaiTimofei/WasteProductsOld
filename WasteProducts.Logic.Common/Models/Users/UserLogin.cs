namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// Entity type for a user's login (i.e. facebook, google).
    /// </summary>
    public class UserLogin
    {
        // TODO Delete from code before merge request if wouldn't required
        /// <summary>
        /// The login provider for the login (i.e. facebook, google).
        /// </summary>
        public virtual string LoginProvider { get; set; }
        
        /// <summary>
        /// Key representing the login for the provider.
        /// </summary>
        public virtual string ProviderKey { get; set; }
    }
}
