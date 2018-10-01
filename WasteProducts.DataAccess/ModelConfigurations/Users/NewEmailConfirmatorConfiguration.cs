using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.ModelConfigurations.Users
{
    public class NewEmailConfirmatorConfiguration : EntityTypeConfiguration<NewEmailConfirmator>
    {
        public NewEmailConfirmatorConfiguration()
        {
            ToTable("NewEmailConfirmators");

            HasKey(c => new { c.UserId, c.Token });
        }
    }
}
