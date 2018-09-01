using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupUserInviteTime : EntityTypeConfiguration<GroupUserInviteTimeDB>
    {
        public GroupUserInviteTime()
        {
            ToTable("GroupUserInviteTime");

            HasKey<int>(x => x.Id);
            Property(x => x.GroupUserDBId).HasColumnName("GroupUser_Id");
            Property(x => x.TimeEntry).IsOptional();
            Property(x => x.TimeExit).IsOptional();
            Property(x => x.Invite).IsOptional();
        }

    }
}
