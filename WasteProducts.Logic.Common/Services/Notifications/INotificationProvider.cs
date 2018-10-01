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
        /// <param name="notification">notification for sending</param>
        /// <param name="usersIds">users ids</param>
        /// <returns></returns>
        Task NotifiyAsync(Notification notification, params string[] usersIds);
    }
}