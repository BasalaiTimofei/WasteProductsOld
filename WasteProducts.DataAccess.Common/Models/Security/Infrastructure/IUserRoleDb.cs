namespace WasteProducts.DataAccess.Common.Models.Security.Infrastructure
{
    public interface IUserRoleDb
    {
        IRoleDb Role { get; set; }
        int RoleId { get; set; }
        IUserDb User { get; set; }
        int UserId { get; set; }
    }
}