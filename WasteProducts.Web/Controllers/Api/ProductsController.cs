using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services.Products;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Controller that performs actions on products and gives the client the corresponding response.
    /// </summary>
    [RoutePrefix("api/products")]
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="productService">Product service.</param>
        /// <param name="logger">NInject logger.</param>
        public ProductsController(IProductService productService, ILogger logger) : base(logger)
        {
            _productService = productService;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>All products from database.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetAll products result", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Products were not found in database")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            return Ok(await _productService.GetAllAsync());
        }

        /// <summary>
        /// Gets product by id.
        /// </summary>
        /// <param name="id">Product's id.</param>
        /// <returns>Product with the specific id.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetById product result", typeof(Product))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect Id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById([FromUri] string id)
        {
            return Ok(await _productService.GetByIdAsync(id));
        }

        /// <summary>
        /// Adds new product. File from request reads by an "image" key, so make sure to use this key in FormData.
        /// </summary>
        /// <returns>Created product.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Product was successfully added", typeof(Product))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect image")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> CreateProduct()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                using(var imageStream = httpRequest.Files["image"].InputStream)
                {
                    var id = await _productService.AddAsync(imageStream);

                    return Created("api/products/" + id, id);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletes the product by id.
        /// </summary>
        /// <param name="id">Id of the product to be deleted.</param>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Product was successfully deleted")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect Id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete([FromUri] string id)
        {
            await _productService.DeleteAsync(id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds the product to specific category.
        /// </summary>
        /// <param name="productId">Product's id.</param>
        /// <param name="categoryId">Id of the category to be added.</param>
        /// <returns>Product with added category.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product was successfully added to category", typeof(Product))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect Id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpPatch, Route("{productId}/category/{categoryId}")]
        public async Task<IHttpActionResult> AddToCategory([FromUri]string productId, [FromUri]string categoryId)
        {
            await _productService.AddToCategoryAsync(productId, categoryId);

            return Ok(await _productService.GetByIdAsync(productId));
        }

        /// <summary>
        /// Updates the product .
        /// </summary>
        /// <param name="data">Data by which the product should be updated.</param>
        /// <returns>Updated product.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product was successfully updated", typeof(Product))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect data")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpPut, Route("")]
        public async Task<IHttpActionResult> UpdateProduct([FromBody]Product data)
        {
            await _productService.UpdateAsync(data);

            return Ok(await _productService.GetByIdAsync(data.Id));
        }
    }

}
