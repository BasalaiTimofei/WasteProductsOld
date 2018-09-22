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
            var item = _groupService.FindById(new Guid(groupId));
            if (item == null)
            {
                return NotFound();
            }
            return await Task.FromResult(Ok(item));
        }

        /// <summary>
        /// Group create
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>201(Group id, Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Group create", typeof(Group))]
        [ApiValidationExceptionFilter]
        [HttpPost, Route("")]
        public IHttpActionResult Create(Group item)
        {
            var groupId = _groupService.Create(item);

            return Created($"{groupId}", item);
        }

        /// <summary>
        /// Group update
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [ApiValidationExceptionFilter]
        [SwaggerResponse(HttpStatusCode.OK, "Group update", typeof(Group))]
        [HttpPut, Route("{groupId}")]
        public IHttpActionResult Update(Group item)
        {
            _groupService.Update(item);

            return Ok(item);
        }

        /// <summary>
        /// Group delete
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>302(url)</returns>
        [SwaggerResponseRemoveDefaults]
        [ApiValidationExceptionFilter]
        [SwaggerResponse(HttpStatusCode.Redirect, "Group delete")]
        [HttpDelete, Route("{groupId}")]
        public IHttpActionResult Delete(Group item)
        {
            _groupService.Delete(item);

            return Redirect($"");
        }
    }
}
