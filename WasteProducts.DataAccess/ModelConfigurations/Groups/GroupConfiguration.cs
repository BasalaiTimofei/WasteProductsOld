using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupConfiguration : EntityTypeConfiguration<GroupDB>
    {
        public GroupConfiguration()
        {
            ToTable("Group");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasMaxLength(50);
            Property(x => x.Information).HasMaxLength(255);
            Property(x => x.Created).IsOptional();
            Property(x => x.Deleted).IsOptional();
            Property(x => x.Modified).IsOptional();
            Property(x => x.IsNotDeleted).IsOptional();

            HasMany(x => x.GroupBoards)
                .WithRequired(y => y.Group)
                .HasForeignKey(z=>z.GroupId);

            HasMany(x => x.GroupUsers)
                .WithRequired(y => y.Group)
                .HasForeignKey(z => z.GroupId);
        }

    }
}
