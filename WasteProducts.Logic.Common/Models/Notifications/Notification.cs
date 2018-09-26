using System;

namespace WasteProducts.Logic.Common.Models.Notifications
{
    public class Notification
    {
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

        /// <summary>
        /// Notification subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Notification message
        /// </summary>
        public string Message { get; set; }
    }
}