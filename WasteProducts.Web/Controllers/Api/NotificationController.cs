using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Notifications;
using WasteProducts.Logic.Common.Services.Notifications;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Api controller for database management
    /// </summary>
    [RoutePrefix("api")]
    [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect query string")]
    [SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized request.")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "Exceptions during the process.")]
    public class NotificationController : BaseApiController
    {
        private readonly INotificationService _notificationService;

        /// <inheritdoc />
        public NotificationController(INotificationService notificationService, ILogger logger) : base(logger)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Returns all user notifications
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        [HttpGet, Route("user/{userId}/notifications/")]
        [SwaggerResponse(HttpStatusCode.OK, "Notifications was fetched.")]
        public async Task<IHttpActionResult> NotifyUser(string userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId).ConfigureAwait(true);

            return Ok(notifications);
        }

        /// <summary>
        /// Sends notification message to user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="notificationMessage">notification message</param>
        /// <returns></returns>
        [HttpPost, Route("user/{userId}/notify")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Notification was sent.")]
        public async Task<IHttpActionResult> NotifyUser(string userId, [FromBody] NotificationMessage notificationMessage)
        {
            await _notificationService.NotifyAsync(notificationMessage, userId).ConfigureAwait(true);

            return StatusCode(HttpStatusCode.NoContent);
        }

        
    }
}
