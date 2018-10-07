
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
    /// Controller management product in group.
    /// </summary>
    [RoutePrefix("api/groups")]
    public class GroupProductController : BaseApiController
    {
        private readonly IGroupProductService _groupProductService;

        /// <summary>
        /// Creates an Instance of GroupProductController.
        /// </summary>
        /// <param name="groupProductService">Instance of GroupProductService from business logic</param>
        /// <param name="logger">Instance of logger</param>
        public GroupProductController(IGroupProductService groupProductService, ILogger logger) : base(logger)
        {
            _groupProductService = groupProductService;
        }

        /// <summary>
        /// Get product object by id 
        /// </summary>
        /// <param name="productId">Primary key</param>
        /// <returns>200(Object) || 404</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get product by product id", typeof(GroupProduct))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Incorrect product")]
        [HttpGet, Route("product/{productId}")]
        public async Task<IHttpActionResult> GetBoard([FromUri]string productId)
        {
            var item = await _groupProductService.FindById(productId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        /// <summary>
        /// Product create
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product create", typeof(GroupProduct))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpPost, Route("board/{boardId}/product")]
        public async Task<IHttpActionResult> Create(GroupProduct item)
        {
            item.Id = await _groupProductService.Create(item);

            return Ok(item);
        }

        /// <summary>
        /// Product update
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product update", typeof(GroupProduct))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpPut, Route("board/product/{productId}")]
        public async Task<IHttpActionResult> Update(GroupProduct item)
        {
            await _groupProductService.Update(item);

            return Ok(item);
        }

        /// <summary>
        /// Product delete
        /// </summary>
        /// <param name="productId">Primary key</param>
        /// <returns>200()</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product delete")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpDelete, Route("board/product/{productId}")]
        public async Task<IHttpActionResult> Delete([FromUri] string productId)
        {
            await _groupProductService.Delete(productId);

            return Ok();
        }
    }
}
