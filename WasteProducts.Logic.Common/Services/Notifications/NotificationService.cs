using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Notifications;

namespace WasteProducts.Logic.Common.Services.Notifications
{
    /// <summary>
    /// Notification service interface
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Gets all user notifications
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>enumerable of user's notifications</returns>
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);

        /// <summary>
        /// Marks user notification as read
        /// </summary>
        /// <param name="userId"> user id</param>
        /// <param name="notificationId">notification id</param>
        /// <returns></returns>
        Task MarkReadAsync(string userId, string notificationId);
    }
}