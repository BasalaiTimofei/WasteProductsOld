namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    public interface IUserLoginDb
    {
        string LoginProvider { get; set; }
        string ProviderKey { get; set; }
        IUserDb User { get; set; }
        int UserId { get; set; }
    }
}