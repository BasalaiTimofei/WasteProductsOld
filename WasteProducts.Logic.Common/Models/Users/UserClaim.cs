namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// EntityType that represents one specific user claim
    /// </summary>
    public class UserClaim
    {
        /// <summary>
        /// Claim type
        /// </summary>
        public string ClaimType { get; set; }
        
        /// <summary>
        /// Claim value
        /// </summary>
        public string ClaimValue { get; set; }

        public override bool Equals(object obj)
            =>
            obj is UserClaim other &&
            this.ClaimType == other.ClaimType &&
            this.ClaimValue == other.ClaimValue;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
