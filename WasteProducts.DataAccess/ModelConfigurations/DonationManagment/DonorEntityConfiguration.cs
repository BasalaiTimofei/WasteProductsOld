using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.DonationManagment;

namespace WasteProducts.DataAccess.ModelConfigurations.DonationManagment
{
    class DonorEntityConfiguration : EntityTypeConfiguration<DonorDB>
    {
        public DonorEntityConfiguration()
        {
            this.ToTable("Donor");

            this.HasKey<string>(d => d.Id);

            this.Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(d => d.Email)
                .HasMaxLength(127);

            this.Property(d => d.FirstName)
                .HasMaxLength(64);

            this.Property(d => d.LastName)
                .HasMaxLength(64);

            this.HasRequired<AddressDB>(d => d.Address)
                .WithMany(a => a.Donors)
                .HasForeignKey(d => d.AddressId);

            this.HasMany<DonationDB>(d => d.Donations);
        }
    }
}