using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class Group : EntityTypeConfiguration<GroupDB>
    {
        public Group()
        {
            ToTable("Group");

            HasKey<int>(x => x.Id);
            Property(x => x.Name).HasMaxLength(50);
            Property(x => x.Information).HasMaxLength(255);
            Property(x => x.Admin).HasColumnName("User_Id");
            Property(x => x.TimeCreate).IsOptional();
            Property(x => x.Deleted).IsOptional();
            Property(x => x.IsDeleted);

            HasMany(x => x.GroupBoardDBs)
                .WithRequired(y => y.GroupDB)
                .Map(m => m.MapKey("GroupBoardId"));

            HasMany(x => x.GroupUserDBs)
                .WithRequired(y => y.GroupDB)
                .Map(m => m.MapKey("GroupUserId"));
        }

    }
}
