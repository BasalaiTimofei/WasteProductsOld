using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Donations;

namespace WasteProducts.DataAccess.ModelConfigurations.Donations
{
    class AddressEntityConfiguration : EntityTypeConfiguration<AddressDB>
    {
        public AddressEntityConfiguration()
        {
            this.ToTable("Address");

            this.HasKey(a => a.Id);

            this.Property(a => a.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(a => a.City)
                .HasMaxLength(40);

            this.Property(a => a.Country)
                .HasMaxLength(64);

            this.Property(a => a.Name)
                .HasMaxLength(128);

            this.Property(a => a.State)
                .HasMaxLength(40);

            this.Property(a => a.Street)
                .HasMaxLength(200);

            this.Property(a => a.Zip)
                .HasMaxLength(20);

            this.HasMany<DonorDB>(a => a.Donors);
        }
    }
}