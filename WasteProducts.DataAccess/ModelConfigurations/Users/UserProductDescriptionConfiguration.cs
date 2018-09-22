using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.ModelConfigurations.Users
{
    public class UserProductDescriptionConfiguration : EntityTypeConfiguration<UserProductDescriptionDB>
    {
        public UserProductDescriptionConfiguration()
        {
            ToTable("ProductDescriptions");

            HasKey(d => new { d.UserId, d.ProductId });

            HasRequired(d => d.User).WithMany(u => u.ProductDescriptions).HasForeignKey(k => k.UserId);

            HasRequired(d => d.Product).WithMany(p => p.UserDescriptions).HasForeignKey(k => k.ProductId);
        }
    }
}
