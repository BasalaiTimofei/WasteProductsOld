namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// EntityType that represents one specific user claim
    /// </summary>
    public class UserClaim
    {
        ///// <summary>
        ///// Primary key
        ///// </summary>
        //public virtual int Id { get; set; }
           
        ///// <summary>
        ///// User Id for the user who owns this login
        ///// </summary>
        //public virtual string UserId { get; set; }
           
        /// <summary>
        /// Claim type
        /// </summary>
        public virtual string ClaimType { get; set; }
        
        /// <summary>
        /// Claim value
        /// </summary>
        public virtual string ClaimValue { get; set; }

        public override bool Equals(object obj)
            =>
            obj is UserClaim other &&
            this.ClaimType == other.ClaimType &&
            this.ClaimValue == other.ClaimValue;
    }
}
