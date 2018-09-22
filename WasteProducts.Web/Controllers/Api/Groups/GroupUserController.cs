using System;
using System.Net;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;
using WasteProducts.Web.ExceptionHandling.Api;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using System.Threading.Tasks;

namespace WasteProducts.Web.Controllers.Api.Groups
{
    /// <summary>
    /// Controller management user in group.
    /// </summary>
    [RoutePrefix("api/groups")]
    public class GroupUserController : BaseApiController
    {
        private readonly IGroupUserService _groupUserService;

        /// <summary>
        /// Creates an Instance of GroupUserController.
        /// </summary>
        /// <param name="groupUserService">Instance of GroupUserService from business logic</param>
        /// <param name="logger">Instance of logger</param>
        public GroupUserController(IGroupUserService groupUserService, ILogger logger) : base(logger)
        {
            _groupUserService = groupUserService;
        }

        /// <summary>
        /// Invite send
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        /// <returns>200</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Invite send", typeof(GroupUser))]
        [HttpPost, Route("{groupId}/invite/{adminId}")]
        public IHttpActionResult Invite(GroupUser item, [FromUri]string adminId)
        {
            _groupUserService.Invite(item, adminId);

            return Ok();
        }

        /// <summary>
        /// User delete
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        /// <returns>200</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "User delete", typeof(GroupUser))]
        [HttpPost, Route("{groupId}/dismiss/{adminId}")]
        public IHttpActionResult Kick(GroupUser item, [FromUri]string adminId)
        {
            _groupUserService.Kick(item, adminId);

            return Ok();
        }

        /// <summary>
        /// Get entitle
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        /// <returns>200</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get entitle", typeof(GroupUser))]
        [HttpPut, Route("{groupId}/entitle/{adminId}")]
        public IHttpActionResult TakeAwayRightToCreateBoards(GroupUser item, [FromUri]string adminId)
        {
            _groupUserService.TakeAwayRightToCreateBoards(item, adminId);

            return Ok();
        }
    }
}
