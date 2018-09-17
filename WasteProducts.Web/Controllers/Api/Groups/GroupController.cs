using NLog;
using Swagger.Net.Annotations;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Web.Controllers.Api.Groups
{
    /// <summary>
    /// Controller create Group.
    /// </summary>
    public class GroupController : BaseApiController
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService, ILogger logger) : base(logger)
        {
            _groupService = groupService;
        }

        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get group", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Incorrect id group")]
        [HttpGet, Route("api/group/{id}")]
        public IHttpActionResult GetGroup(string id)
        {
            var item = _groupService.FindById(new Guid(id));
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get group", typeof(Group))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Incorrect id group")]
        [HttpGet, Route("api/group/{id}/{userid}")]
        public IHttpActionResult GetGroup(string id, string userid)
        {
            var item = _groupService.FindByAdmin(userid);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Request processed", typeof(Group))]
        [HttpPost, Route("api/group")]
        public IHttpActionResult CreateGroup([FromBody]Group item)
        {
            item.GroupBoards = null;
            item.GroupUsers = null;
            _groupService.Create(item);

            return Ok();
        }

        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Request processed", typeof(Group))]
        [HttpPut, Route("api/group/{id}")]
        public IHttpActionResult UpdateGroup(string id, [FromBody]Group item)
        {
            item.Id = id.ToString();
            item.GroupBoards = null;
            item.GroupUsers = null;
            _groupService.Update(item);

            return Ok();
        }

        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Request processed")]
        [HttpDelete, Route("api/group/{id}")]
        public IHttpActionResult DeleteGroup(string id, [FromBody]Group item)
        {
            item.Id = id.ToString();
            item.GroupBoards = null;
            item.GroupUsers = null;
            _groupService.Delete(item);

            return Ok();
        }
    }
}
