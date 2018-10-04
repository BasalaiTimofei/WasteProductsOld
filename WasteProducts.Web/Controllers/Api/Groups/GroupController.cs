
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

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
        [SwaggerResponse(HttpStatusCode.OK, "Get group by group id", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Incorrect id user")]
        [HttpGet, Route("groups/{groupId}")]
        public async Task<IHttpActionResult> GetGroup(string groupId)
        {
            using (_groupService)
            {
                var item = await _groupService.FindById(groupId);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
        }

        /// <summary>
        /// Get group object by id user
        /// </summary>
        /// <param name="userId">Primary key</param>
        /// <returns>200(Object) || 404</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get user group by user id", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Incorrect id user")]
        [HttpGet, Route("user/{userId}/group")]
        public async Task<IHttpActionResult> GetUserGroup(string userId)
        {
            using (_groupService)
            {
                var item = await _groupService.FindByAdmin(userId);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
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
            using (_groupService)
            {
                var group = await _groupService.Create(item);

                var uriBuilder = new UriBuilder(Request.RequestUri);
                uriBuilder.Path += $"/{group.Id}";

                return Created(uriBuilder.Uri, group);
            }
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Group delete
        /// </summary>
        /// <param name="groupId">Primary key</param>
        /// <returns>302(url)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Redirect, "Group delete")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpDelete, Route("groups/{groupId}")]
        public async Task<IHttpActionResult> Delete([FromUri]string groupId)
        {
            await _groupService.Delete(groupId);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
