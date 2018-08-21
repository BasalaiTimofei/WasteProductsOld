using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security;
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
