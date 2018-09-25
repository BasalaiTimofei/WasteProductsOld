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
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services.Products;

namespace WasteProducts.Web.Controllers.Api
{
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

        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetAll product result", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Products not found in database")]
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            if (products.Count() == 0)
            {
                return await Task.FromResult(NotFound());
            }

            return await Task.FromResult(Ok(products));
        }


        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetByCategory products result", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Products with specified category were not found in the database")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect category")]
        [HttpGet, Route("{id}")]
        public void GetById(string id)
        {

        }

        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetByCategory products result", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect category")]
        [HttpPost, Route("")]
        public void CreateProduct() //stream
        {
            // Погуглить как получать картинку во входящих параметрах [FromBody?]
            Stream imageStream = HttpContext.Current.Request.Files[0]?.InputStream;
            var id = await _productService.Add(imageStream);

            return await Task.FromResult(Ok(id));
        }

        [HttpDelete, Route("{id}")]
        public void Delete(string id)
        {

        }

        [HttpPatch, Route("{productId}/category/{categoryId}")]
        public void AddToCategory(string productId, string categoryId)
        {

        }

        [HttpPut, Route("")]
        public void UpdateProduct([FromBody]Product data)
        {

        }
    }

}