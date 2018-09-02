using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupUser : EntityTypeConfiguration<GroupUserDB>
    {
        public GroupUser()
        {
            ToTable("GroupUser");

            HasKey<int>(x => x.Id);
            Property(x => x.GroupDBId).HasColumnName("Group_Id");
            Property(x => x.UserId).HasColumnName("User_Id");
            Property(x => x.Bool);

            HasMany(x => x.GroupUserInviteTimeDBs)
                .WithRequired(y=>y.GroupUserDB)
                .HasForeignKey(z=>z.GroupUserDBId);
        }

    }
}
