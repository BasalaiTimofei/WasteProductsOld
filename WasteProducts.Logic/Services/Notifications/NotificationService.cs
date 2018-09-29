using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Notifications;
using WasteProducts.Logic.Common.Services.Notifications;

namespace WasteProducts.Logic.Services.Notifications
{
    /// <inheritdoc />
    public class NotificationService : INotificationService
    {
        private readonly IEnumerable<INotificationProvider> _notificationProviders;

        /// <inheritdoc />
        public NotificationService(IEnumerable<INotificationProvider> notificationProviders)
        {
            _notificationProviders = notificationProviders;
        }

        /// <inheritdoc />
        public Task NotifyAsync(NotificationMessage notificationMessage, params string[] usersIds)
        {
            //TODO: cast Notification message to Notification 

            //todo: add notification to db

            //todo: send notification using providers

            return Task.CompletedTask; // todo: to remove after implementation of method
        }

        /// <inheritdoc />
        public Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return Task.FromResult(new []{new Notification()
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.UtcNow,
                Subject = "SOme Subject",
                Message = "Some Message"
            }}.AsEnumerable());
        }

        /// <inheritdoc />
        public Task MarkReadAsync(string userId, string notificationId)
        {
            //todo mark notification as read for user

            return Task.CompletedTask;// todo: to remove after implementation of method
        }
    }
}