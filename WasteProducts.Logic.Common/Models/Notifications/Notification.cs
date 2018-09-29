using System;

namespace WasteProducts.Logic.Common.Models.Notifications
{
    /// <summary>
    /// Notification
    /// </summary>
    public class Notification: NotificationMessage
    {
        /// <summary>
        /// Creates new Notification
        /// </summary>
        public Notification() {  }

        /// <summary>
        /// Creates new Notification from NotificationMessage
        /// </summary>
        /// <param name="notificationMessage">notification message</param>
        public Notification(NotificationMessage notificationMessage)
        {
            this.Date = DateTime.UtcNow; ;
            this.Subject = notificationMessage.Subject;
            this.Message = notificationMessage.Message;
        }

        /// <summary>
        /// Unique identifier of concrete Notification
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Is notification read flag
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Notification date
        /// </summary>
        public DateTime Date { get; set; }
    }
}