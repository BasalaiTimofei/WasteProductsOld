using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Notifications;
using WasteProducts.Logic.Common.Services.Notifications;
using WasteProducts.Web.Hubs;

namespace WasteProducts.Web.Utils.Hubs
{
    /// <summary>
    /// SignalR Notification provider
    /// </summary>
    public class SignalRNotifiactionProvider : INotificationProvider
    {
        /// <inheritdoc />
        public Task NotificateAsync(string userId, Notification notification)
        {
            return Task.Run(() => NotificationHub.SendNotification(userId, notification));
        }
    }
}