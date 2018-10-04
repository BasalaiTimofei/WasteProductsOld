
using System.Net;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using System.Threading.Tasks;

namespace WasteProducts.Web.Controllers.Api.Groups
{
    /// <summary>
    /// ApiController management Group.
    /// </summary>
    [RoutePrefix("api")]
    public class GroupController : BaseApiController
    {
        private IGroupService _groupService;

        /// <summary>
        /// Creates an Instance of GroupController.
        /// </summary>
        /// <param name="groupService">Instance of GroupService from business logic</param>
        /// <param name="logger">Instance of logger</param>
        public GroupController(IGroupService groupService, ILogger logger) : base(logger)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// Get group object by id user
        /// </summary>
        /// <param name="groupId">Primary key</param>
        /// <returns>200(Object) || 404</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get group", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Incorrect id user")]
        [HttpGet, Route("groups/{groupId}")]
        public async Task<IHttpActionResult> GetGroupByGroupId(string groupId)
        {
            var item = await _groupService.FindById(groupId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        /// <summary>
        /// Get group object by id user
        /// </summary>
        /// <param name="userId">Primary key</param>
        /// <returns>200(Object) || 404</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get group", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Incorrect id user")]
        [HttpGet, Route("user/{userId}/group")]
        public async Task<IHttpActionResult> GetGroupByUserId(string userId)
        {
            var item = await _groupService.FindByAdmin(userId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        /// <summary>
        /// Group create
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>201(Group id, Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Group create", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpPost, Route("groups")]
        public async Task<IHttpActionResult> Create(Group item)
        {
            var groupId = await _groupService.Create(item);

            return Created($"{groupId}", item);
        }

        /// <summary>
        /// Group update
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Group update", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpPut, Route("groups/{groupId}")]
        public async Task<IHttpActionResult> Update(Group item)
        {
            await _groupService.Update(item);

            return Ok(item);
        }

        /// <summary>
        /// Group delete
        /// </summary>
        /// <param name="groupId">Primary key</param>
        /// <param name="adminId">Primary key</param>
        /// <returns>302(url)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Redirect, "Group delete")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpDelete, Route("groups/{groupId}/{adminId}")]
        public async Task<IHttpActionResult> Delete([FromUri]string groupId, [FromUri]string adminId)
        {
            var item = new Group { Id = groupId, AdminId = adminId };
            await _groupService.Delete(item);

            return Redirect($"");
        }
    }
}
