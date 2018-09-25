using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Notifications;
using WasteProducts.Logic.Common.Services.Notifications;

namespace WasteProducts.Logic.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IEnumerable<INotificationProvider> _notificationProviders;

        /// <inheritdoc />
        public NotificationService(IEnumerable<INotificationProvider> notificationProviders)
        {
            _notificationProviders = notificationProviders;
        }

        /// <inheritdoc />
        public Task NotificateUserAsync(string userId, Notification notification)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task NotificateUsersAsync(Notification notification, params string[] usersIds)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task MarkReadAsync(string userId, string notificationId)
        {
            throw new System.NotImplementedException();
        }
    }
}