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
    [RoutePrefix("api/groups")]
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
        /// Get group object by id group
        /// </summary>
        /// <param name="groupId">Primary key</param>
        /// <returns>200(Object) || 404</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get group", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Incorrect id group")]
        [HttpGet, Route("{groupId}", Name = "GetGroup")]
        public async Task<IHttpActionResult> GetGroupById(string groupId)
        {
            var item = await _groupService.FindById(groupId);
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
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Create(Group item)
        {
            var groupId = await _groupService.Create(item);

            return Ok(Created($"{groupId}", item));
        }

        /// <summary>
        /// Group update
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Group update", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpPut, Route("{groupId}")]
        public async Task<IHttpActionResult> Update(Group item)
        {
            await _groupService.Update(item);

            return Ok(item);
        }

        /// <summary>
        /// Group delete
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>302(url)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Redirect, "Group delete")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpDelete, Route("{groupId}")]
        public async Task<IHttpActionResult> Delete(Group item)
        {
            await _groupService.Delete(item);

            return Redirect($"");
        }
    }
}
