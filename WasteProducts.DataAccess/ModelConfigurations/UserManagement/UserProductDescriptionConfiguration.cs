using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.ModelConfigurations.UserManagement
{
    public class UserProductDescriptionConfiguration : EntityTypeConfiguration<UserProductDescriptionDB>
    {
        public UserProductDescriptionConfiguration()
        {
            HasKey(d => new { d.UserId, d.ProductId });

            HasRequired(d => d.User).WithMany(u => u.ProductDescriptions).HasForeignKey(k => k.UserId);

            HasRequired(d => d.Product).WithMany(p => p.UserDescriptions).HasForeignKey(k => k.ProductId);
        }
    }
}
