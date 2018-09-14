using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupCommentConfiguration : EntityTypeConfiguration<GroupCommentDB>
    {
        public GroupCommentConfiguration()
        {
            ToTable("GroupComment");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Comment);
            Property(x => x.Modified).IsOptional();
            Property(x => x.GroupBoardId).IsRequired();
            Property(x => x.CommentatorId).IsRequired();
        }

    }
}
