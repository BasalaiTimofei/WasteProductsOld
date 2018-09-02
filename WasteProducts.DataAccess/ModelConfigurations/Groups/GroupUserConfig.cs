using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupUserConfig : EntityTypeConfiguration<GroupUserDB>
    {
        public GroupUserConfig()
        {
            ToTable("GroupUser");

            HasKey<int>(x => x.Id);
            Property(x => x.IsInvited).IsOptional();
            Property(x => x.RigtToCreateBoards).IsOptional();
            Property(x => x.Modified).IsOptional();
        }

    }
}
