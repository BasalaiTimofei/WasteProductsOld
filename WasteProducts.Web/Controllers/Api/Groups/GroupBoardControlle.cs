using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Web.Controllers.Api.Groups
{
    /// <summary>
    /// Controller management board.
    /// </summary>
    [RoutePrefix("api/groups")]
    public class GroupBoardController : BaseApiController
    {
        private readonly IGroupBoardService _groupBoardService;

        /// <summary>
        /// Creates an Instance of GroupBoardController.
        /// </summary>
        /// <param name="groupBoardService">Instance of GroupBoardService from business logic</param>
        /// <param name="logger">Instance of logger</param>
        public GroupBoardController(IGroupBoardService groupBoardService, ILogger logger) : base(logger)
        {
            _groupBoardService = groupBoardService;
        }

        /// <summary>
        /// Board create
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Board create", typeof(GroupBoard))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpPost, Route("{groupId}/board")]
        public async Task<IHttpActionResult> Create(GroupBoard item)
        {
            item.Id = await _groupBoardService.Create(item);

            return Ok(item);
        }

        /// <summary>
        /// Board update
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Board update", typeof(GroupBoard))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpPut, Route("{groupId}/board")]
        public async Task<IHttpActionResult> Update(GroupBoard item)
        {
            await _groupBoardService.Update(item);

            return Ok(item);
        }

        /// <summary>
        /// Board delete
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200()</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Board delete")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpDelete, Route("{groupId}/board")]
        public async Task<IHttpActionResult> Delete(GroupBoard item)
        {
            await _groupBoardService.Delete(item);

            return Ok();
        }
    }
}
