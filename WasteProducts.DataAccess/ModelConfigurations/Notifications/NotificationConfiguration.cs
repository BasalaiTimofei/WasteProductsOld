using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WasteProducts.DataAccess.Common.Models.Notifications;

namespace WasteProducts.DataAccess.ModelConfigurations.Notifications
{
    public class NotificationConfiguration : EntityTypeConfiguration<NotificationDB>
    {
        public NotificationConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(e => e.User);

            Property(e => e.IsRead).IsRequired();
            Property(e => e.Date).IsRequired();
            Property(e => e.Subject).HasMaxLength(256).IsVariableLength();
            Property(e => e.Message).IsMaxLength().IsVariableLength();
            
        }
    }
}