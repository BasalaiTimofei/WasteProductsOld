using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Barcods;
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
        /// <param name="logger">NLog logger.</param>
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
            return Ok(await _productService.GetAll());
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
            return Ok(await _productService.GetById(id));
        }

        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Product was successfully added", typeof(Product))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect image")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> CreateProduct()
        {
            var image = HttpContext.Current.Request.Files[0].InputStream;

            var id = await _productService.Add(image);
            if (id == null) return BadRequest();

            return Created("api/products/" + id, GetById(id));
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
        public async Task Delete([FromUri] string id)
        {
            await _productService.Delete(id);
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
            await _productService.AddToCategory(productId, categoryId);

            return Ok(GetById(productId));
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
            await _productService.Update(data);

            return Ok(GetById(data.Id));
        }
    }

}
