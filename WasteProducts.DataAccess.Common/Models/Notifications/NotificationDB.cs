using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models.Notifications
{
    /// <summary>
    /// Notification model for services
    /// </summary>
    public class NotificationDB
    {
        /// <summary>
        /// Notification Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Users recipient of notification
        /// </summary>
        public virtual UserDB User { get; set; }

        /// <summary>
        /// Is read flag
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Message in notification
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Subject of notification
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Date of notification
        /// </summary>
        public DateTime Date { get; set; }
    }
}