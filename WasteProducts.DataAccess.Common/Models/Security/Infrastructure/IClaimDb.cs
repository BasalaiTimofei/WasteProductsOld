namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    public interface IClaimDb
    {
        int ClaimId { get; set; }
        string ClaimType { get; set; }
        string ClaimValue { get; set; }
        IUserDb User { get; set; }
        int UserId { get; set; }
    }
}