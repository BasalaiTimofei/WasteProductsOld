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
        public Task NotifiyAsync(Notification notification,params string[] usersIds)
        {
            return Task.Run(() =>
            {
                foreach (var userId in usersIds)
                {
                    NotificationHub.SendNotification(userId, notification);
                }
            });
        }
    }
}