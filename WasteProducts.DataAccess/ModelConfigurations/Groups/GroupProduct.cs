using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupProduct : EntityTypeConfiguration<GroupProductDB>
    {
        public GroupProduct()
        {
            ToTable("GroupProduct");

            HasKey<int>(x => x.Id);
            Property(x => x.GroupBoardDBId).HasColumnName("GroupBoard_Id");
            Property(x => x.ProductId).HasColumnName("Product_Id");
            Property(x => x.Information).HasMaxLength(255);
        }

    }
}
