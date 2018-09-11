using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupUserConfiguration : EntityTypeConfiguration<GroupUserDB>
    {
        public GroupUserConfiguration()
        {
            ToTable("GroupUser");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.IsInvited).IsOptional();
            Property(x => x.RightToCreateBoards).IsOptional();
            Property(x => x.Modified).IsOptional();
            Property(x => x.GroupId).IsRequired();
            Property(x => x.UserId).IsRequired();
        }

    }
}
