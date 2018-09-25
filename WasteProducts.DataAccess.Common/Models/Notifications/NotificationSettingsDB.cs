namespace WasteProducts.DataAccess.Common.Models.Notifications
{
    /// <summary>
    /// Notifications settings
    /// </summary>
    public class NotificationSettingsDB
    {
        /// <summary>
        /// Is user subscribed on Email notifications
        /// </summary>
        public bool EmailNotificationEnabled { get; set; }

        /// <summary>
        /// Is user subscribed on Sms notifications
        /// </summary>
        public bool SmsNotificationEnabled { get; set; }

        /// <summary>
        /// Is user subscribed on Browser notifications
        /// </summary>
        public bool BrowserNotificationEnabled { get; set; }
    }
}