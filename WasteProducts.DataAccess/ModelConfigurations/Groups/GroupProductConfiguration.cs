using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupProductConfiguration : EntityTypeConfiguration<GroupProductDB>
    {
        public GroupProductConfiguration()
        {
            ToTable("GroupProducts");

            HasKey(x => x.Id);
            Property(x => x.Information).HasMaxLength(255);
            Property(x => x.Modified).IsOptional();
            Property(x => x.GroupBoardId).IsRequired();
            Property(x => x.ProductId).IsRequired();
        }

    }
}
