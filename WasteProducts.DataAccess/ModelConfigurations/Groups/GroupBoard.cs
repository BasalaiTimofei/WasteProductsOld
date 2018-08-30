using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models;

namespace WasteProducts.DataAccess.ModelConfigurations
{
    public class GroupBoard : EntityTypeConfiguration<GroupBoardDB>
    {
        public GroupBoard()
        {
            ToTable("GroupBoard");

            HasKey<int>(x => x.Id);
            Property(x => x.GroupDBId).HasColumnName("Group_Id");
            Property(x => x.UserId).HasColumnName("User_Id");
            Property(x => x.Name).HasMaxLength(50);
            Property(x => x.Information).HasMaxLength(255);
            Property(x => x.TimeCreate).IsOptional();
            Property(x => x.TimeDelete).IsOptional();
            Property(x => x.Bool);

            HasMany(x => x.GroupProductDBs)
                .WithRequired(y => y.GroupBoardDB)
                .HasForeignKey(z => z.GroupBoardDBId);
        }

    }
}
