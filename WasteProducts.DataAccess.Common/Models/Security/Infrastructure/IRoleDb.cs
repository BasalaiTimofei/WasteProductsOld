namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    public interface IRoleDb
    {
        string Name { get; set; }
        int RoleId { get; set; }
        System.Collections.Generic.ICollection<IUserDb> Users { get; set; }
    }
}