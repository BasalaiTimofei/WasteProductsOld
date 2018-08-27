using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Contexts.Security.Configurations
{
    class UserLoginConfiguration : EntityTypeConfiguration<IUserLoginDb>
    {
        public UserLoginConfiguration()
        {
            ToTable("UserLogins");
            HasKey(c => new
            {
                c.LoginProvider,
                c.ProviderKey,
                c.UserId
            });
        }
    }
}
