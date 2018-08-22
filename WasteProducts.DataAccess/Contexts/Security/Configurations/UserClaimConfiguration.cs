using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Contexts.Security.Configurations
{
    class UserClaimConfiguration : EntityTypeConfiguration<IClaimDb>
    {
        public UserClaimConfiguration()
        {
            ToTable("UserClaims");
        }
    }
}
