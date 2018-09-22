using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using System;
using System.Net;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;
using WasteProducts.Web.ExceptionHandling.Api;

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
        [HttpPost, Route("{groupId}/board")]
        public IHttpActionResult Create(GroupBoard item)
        {
            item.Id = _groupBoardService.Create(item);

            return Ok(item);
        }

        /// <summary>
        /// Board update
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Board update", typeof(GroupBoard))]
        [HttpPut, Route("{groupId}/board")]
        public IHttpActionResult Update(GroupBoard item)
        {
            _groupBoardService.Update(item);

            return Ok(item);
        }

        /// <summary>
        /// Board delete
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200()</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Board delete")]
        [HttpDelete, Route("{groupId}/board")]
        public IHttpActionResult Delete(GroupBoard item)
        {
            _groupBoardService.Delete(item);

            return Ok();
        }
    }
}
