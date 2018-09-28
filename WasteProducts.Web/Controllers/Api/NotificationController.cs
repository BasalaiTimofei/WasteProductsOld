using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
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

        [HttpPost, Route("user/{userId}/notificate")]
        [SwaggerResponse(HttpStatusCode.NoContent, "Notification was sent.")]
        public async Task<IHttpActionResult> NotificateUser(string userId, [FromBody] Notification notification)
        {
            await _notificationService.NotificateUserAsync(userId, notification).ConfigureAwait(true);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
