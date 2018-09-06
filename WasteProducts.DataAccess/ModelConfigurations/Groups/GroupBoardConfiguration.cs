using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupBoardConfiguration : EntityTypeConfiguration<GroupBoardDB>
    {
        public GroupBoardConfiguration()
        {
            ToTable("GroupBoard");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasMaxLength(50);
            Property(x => x.Information).HasMaxLength(255);
            Property(x => x.Created).IsOptional();
            Property(x => x.Deleted).IsOptional();
            Property(x => x.Modified).IsOptional();
            Property(x => x.IsDeleted).IsOptional();

            HasMany(x => x.GroupProducts)
                .WithRequired(y => y.GroupBoard)
                .HasForeignKey(z => z.GroupBoardId);

            HasMany(x => x.GroupComments)
                .WithRequired(y => y.GroupBoard)
                .HasForeignKey(z => z.GroupBoardId);
        }

    }
}
