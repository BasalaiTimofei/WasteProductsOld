using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupBoardConfiguration : EntityTypeConfiguration<GroupBoardDB>
    {
        public GroupBoardConfiguration()
        {
            ToTable("GroupBoard");

            HasKey<int>(x => x.Id);
            Property(x => x.Name).HasMaxLength(50);
            Property(x => x.Information).HasMaxLength(255);
            Property(x => x.Created).IsOptional();
            Property(x => x.Deleted).IsOptional();
            Property(x => x.Modified).IsOptional();
            Property(x => x.IsDeleted).IsOptional();

            HasMany(x => x.GroupProducts)
                .WithRequired(y => y.GroupBoard)
                .Map(m => m.MapKey("GroupBoard_Id"));

            HasMany(x => x.GroupComments)
                .WithRequired(y => y.GroupBoard)
                .Map(m => m.MapKey("GroupBoard_Id"));
        }

    }
}
