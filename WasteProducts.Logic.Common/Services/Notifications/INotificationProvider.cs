using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Notifications;

namespace WasteProducts.Logic.Common.Services.Notifications
{
    /// <summary>
    /// Interfaces for injections in Notification service
    /// </summary>
    public interface INotificationProvider
    {
        /// <summary>
        /// Send notification asynchronously
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="notification">notification for sending</param>
        /// <returns></returns>
        Task NotificateAsync(string userId, Notification notification);
    }
}