using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.ModelConfigurations.Users
{
    public class UserConfiguration : EntityTypeConfiguration<UserDB>
    {
        public UserConfiguration()
        {
            HasMany(u => u.Friends).WithMany()
                .Map(t => t.MapLeftKey("UserId").MapRightKey("FriendId").ToTable("UserFriends"));

            HasMany(u => u.Notifications).WithRequired(n => n.User);
        }
    }
}