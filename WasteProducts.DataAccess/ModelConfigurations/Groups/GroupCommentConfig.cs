using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupCommentConfig : EntityTypeConfiguration<GroupCommentDB>
    {
        public GroupCommentConfig()
        {
            ToTable("GroupComment");

            HasKey<string>(x => x.Id);
            Property(x => x.Comment);
            Property(x => x.Modified).IsOptional();
        }

    }
}
