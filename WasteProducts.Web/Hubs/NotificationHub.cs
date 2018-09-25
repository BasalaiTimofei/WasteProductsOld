using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using WasteProducts.Logic.Common.Models.Notifications;
using WasteProducts.Logic.Common.Services.Notifications;
using WasteProducts.Web.Utils.Hubs;

namespace WasteProducts.Web.Hubs
{
    /// <summary>
    /// SignalR notification hub
    /// </summary>
    public class NotificationHub : Hub<NotificationHub.INotifictionClientContract>
    {
        private static readonly HubConnectionManager<string> Connections = new HubConnectionManager<string>();
        private static readonly IHubContext<INotifictionClientContract> HubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub, INotifictionClientContract>();

        private readonly INotificationService _notificationService;

        /// <summary>
        /// Creates new <see cref="NotificationHub"/> with notification service argument
        /// </summary>
        /// <param name="notificationService">notification service</param>
        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <inheritdoc />
        public override Task OnConnected()
        {
            var userId = GetClientId();

            Connections.Add(userId, Context.ConnectionId);

            return SendAllNotificationsAsync(userId);
        }

        /// <inheritdoc />
        public override Task OnReconnected()
        {
            var userId = GetClientId();

            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                Connections.Add(userId, Context.ConnectionId);
            }

            return SendAllNotificationsAsync(userId);
        }

        /// <inheritdoc />
        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = GetClientId();

            Connections.Remove(userId, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Marks notification for user as read
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="notificationId">notification id</param>
        /// <returns>task</returns>
        public Task MarkReadAsync(string userId, string notificationId)
        {
            return _notificationService.MarkReadAsync(userId, notificationId);
        }

        private async Task SendAllNotificationsAsync(string userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId).ConfigureAwait(true);

            Clients.Caller.ReciveNotifications(notifications);
        }

        private string GetClientId()
        {
            return Context.User.Identity.GetUserId() ?? Context.ConnectionId; // TODO: only for tests '?? Context.ConnectionId '
        }

        /// <summary>
        /// Sends notification to user if he has any established connections to hub
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="notification">notification for sending</param>
        public static void SendNotification(string userId, Notification notification)
        {
            var userConnections = Connections.GetConnections(userId).ToList();

            HubContext.Clients.Clients(userConnections).ReciveNotification(notification);
        }

        /// <summary>
        /// Sends notification to user if he has any established connections to hub
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="notifications">notification list for sending</param>
        public static void SendNotifications(string userId, IEnumerable<Notification> notifications)
        {
            var userConnections = Connections.GetConnections(userId).ToList();

            HubContext.Clients.Clients(userConnections).ReciveNotifications(notifications);
        }

        /// <summary>
        /// Hub client contract
        /// </summary>
        public interface INotifictionClientContract
        {
            /// <summary>
            /// Recives enumerable of notifications
            /// </summary>
            /// <param name="notifications">enumerable of notifications</param>
            void ReciveNotifications(IEnumerable<Notification> notifications);

            /// <summary>
            /// Recives notification
            /// </summary>
            /// <param name="notification">notification</param>
            void ReciveNotification(Notification notification);
        }
    }
}