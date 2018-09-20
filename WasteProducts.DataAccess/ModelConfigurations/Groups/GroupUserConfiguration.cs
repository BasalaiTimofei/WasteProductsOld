using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupUserConfiguration : EntityTypeConfiguration<GroupUserDB>
    {
        public GroupUserConfiguration()
        {
            ToTable("GroupUsers");
            HasKey(x => new { x.GroupId, x.UserId });

            HasRequired(g => g.User).WithMany(u => u.Groups).HasForeignKey(k => k.UserId);
            HasRequired(g => g.Group).WithMany(g => g.GroupUsers).HasForeignKey(k => k.GroupId);

            Property(x => x.Created).IsRequired();
            Property(x => x.Modified).IsOptional();
        }
    }
}
