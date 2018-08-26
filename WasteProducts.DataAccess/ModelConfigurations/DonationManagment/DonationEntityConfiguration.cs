using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.DonationManagment;

namespace WasteProducts.DataAccess.ModelConfigurations.DonationManagment
{
    class DonationEntityConfiguration : EntityTypeConfiguration<DonationDB>
    {
        public DonationEntityConfiguration()
        {
            this.ToTable("Donation");

            this.HasKey<string>(d => d.TransactionId);

            this.Property(d => d.TransactionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(d => d.Currency)
                .HasMaxLength(3);
            
            this.HasRequired<DonorDB>(d => d.Donor)
                .WithMany(donor => donor.Donations)
                .HasForeignKey(d => d.DonorId);
        }
    }
}