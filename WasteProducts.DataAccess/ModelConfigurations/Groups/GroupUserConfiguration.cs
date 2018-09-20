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

            Property(x => x.GroupId).IsRequired();
            Property(x => x.UserId).IsRequired();

            Property(x => x.Created).IsRequired();
            Property(x => x.Modified).IsOptional();
        }
    }
}
