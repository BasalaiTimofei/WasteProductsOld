using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupProductConfiguration : EntityTypeConfiguration<GroupProductDB>
    {
        public GroupProductConfiguration()
        {
            ToTable("GroupProduct");

            HasKey<int>(x => x.Id);
            Property(x => x.Information).HasMaxLength(255);
            Property(x => x.Modified).IsOptional();
        }

    }
}
