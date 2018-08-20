namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// Entity type for a user's login (i.e. facebook, google).
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// The login provider for the login (i.e. facebook, google).
        /// </summary>
        public virtual string LoginProvider { get; set; }
        
        /// <summary>
        /// Key representing the login for the provider.
        /// </summary>
        public virtual string ProviderKey { get; set; }

        public override bool Equals(object obj)
            =>
            obj is UserLogin other &&
            this.LoginProvider == other.LoginProvider &&
            this.ProviderKey == other.ProviderKey;
    }
}
