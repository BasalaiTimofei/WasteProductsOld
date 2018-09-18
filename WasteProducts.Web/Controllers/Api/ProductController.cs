using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
    [RoutePrefix("api/product")]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="productService">Product service.</param>
        /// <param name="logger">NLog logger.</param>
        public ProductController(IProductService productService, ILogger logger) : base(logger)
        {
            _productService = productService;
        }

        /// <summary>
        /// Gets product by its name.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <returns>Product with the specific name.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetByName product result", typeof(Product))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product not found in database")]
        [HttpGet, Route("name")]
        public async Task<IHttpActionResult> GetProductByName([FromBody]string name)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest();

            var product = await _productService.GetByNameAsync(name);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Gets products by a category.
        /// </summary>
        /// <param name="category">Category whose products are to be listed.</param>
        /// <returns>Product collection of the specific category.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetByCategory products result", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Products with specified category were not found in the database")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect category")]
        [HttpGet, Route("category")]
        public IHttpActionResult GetProductsByCategory([FromBody]Category category)
        {
            if (category == null) return BadRequest();

            IEnumerable<Product> products = _productService.GetByCategory(category);

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        /// <summary>
        /// Tries to add a new product by name and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="product">The product to be added.</param>
        /// <returns>Represents added product.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Product was successfully added", typeof(Product))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect product")]
        [HttpPost, Route("")]
        public IHttpActionResult AddProduct([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            if (_productService.Add((product), out var addedProduct))
            {
                return Created("", product);
            }
        }

        /// <summary>
        /// Tries to add a new product by name and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="name">The name of the product to be added.</param>
        /// <returns>Represents added product.</returns>
        //[SwaggerResponseRemoveDefaults]
        //[SwaggerResponse(HttpStatusCode.Created, "Product was successfully added", typeof(Product))]
        //[SwaggerResponse(HttpStatusCode.Forbidden, "Products with specified name already exists")]
        //[SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect name")]
        //[HttpPost, Route("add-product")]
        //public IHttpActionResult AddProduct([FromBody]string name)
        //{
        //    if (String.IsNullOrEmpty(name))
        //    {
        //        return BadRequest();
        //    }

        //    if (_productService.Add((name), out var addedProduct))
        //    {
        //        return Content(HttpStatusCode.Created, addedProduct);
        //    }
        //    else
        //    {
        //        return Content(HttpStatusCode.Forbidden, "Products with specified name already exists");
        //    }
        //}

        /// <summary>
        /// Tries to add a new product by barcode and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="barcode">Barcode of the product to be added.</param>
        /// <returns>Represents added product.</returns>
        //[SwaggerResponseRemoveDefaults]
        //[SwaggerResponse(HttpStatusCode.Created, "Product was successfully added", typeof(Product))]
        //[SwaggerResponse(HttpStatusCode.Forbidden, "Products with specified barcode already exists")]
        //[SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect barcode")]
        //[HttpPost, Route("add-product")]
        //public IHttpActionResult AddProduct([FromBody]Barcode barcode)
        //{
        //    if (barcode == null) return BadRequest();

        //    if (_productService.Add((barcode), out var addedProduct))
        //    {
        //        return Content(HttpStatusCode.Created, addedProduct);
        //    }
        //    else
        //    {
        //        return Content(HttpStatusCode.Forbidden, "Product with specified barcode already exists");
        //    }
        //}

        /// <summary>
        /// Tries to delete the product by name and returns whether the deletion is successful or not.
        /// </summary>
        /// <param name="name">The name of the product to be deleted.</param>
        /// <returns>Represents whether the deletion is successful or not.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product was successfully deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product with specified name not found in the database")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect name")]
        [HttpDelete, Route("delete-product")]
        public IHttpActionResult DeleteProduct([FromBody]string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            if (_productService.Delete(name))
            {
                return Content(HttpStatusCode.OK, "Product was successfully deleted");
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Tries to delete the product by barcode and returns whether the deletion is successful or not.
        /// </summary>
        /// <param name="barcode">Barcode of the product to be deleted.</param>
        /// <returns>Represents whether the deletion is successful or not.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product was successfully deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product with specified barcode not found in the database")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect barcode")]
        [HttpDelete, Route("{id}")]
        public IHttpActionResult DeleteProduct(string id)
        {
            if (barcode == null) return BadRequest();

            if (_productService.Delete(barcode))
            {
                return Content(HttpStatusCode.OK, "Product was successfully deleted");
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Tries to add the specific category and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="product">The specific product to add category.</param>
        /// <param name="category">The specific category to be added.</param>
        /// <returns>Represents the product with added category or "Not Found" in case of failure.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "AddCategory to product result", typeof(Product))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product not found in the database")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect product or category")]
        [HttpPut, Route("add-category")]
        public IHttpActionResult AddCategory([FromBody]Product product, [FromBody]Category category)
        {
            if (product == null || category == null) return BadRequest();

            if (_productService.AddCategory(product, category))
            {
                return GetProductByName(product.Name);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Tries to remove the specific category and returns whether the removal is successful or not.
        /// </summary>
        /// <param name="product">The specific product to remove category.</param>
        /// <param name="category">The specific category to be removed.</param>
        /// <returns>Represents the product with deleted category or "Not Found" in case of failure.</returns>
        //[SwaggerResponseRemoveDefaults]
        //[SwaggerResponse(HttpStatusCode.OK, "RemoveCategory result", typeof(Product))]
        //[SwaggerResponse(HttpStatusCode.NotFound, "Product not found in the database")]
        //[SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect product or category")]
        //[HttpDelete, Route("delete-category")]
        //public IHttpActionResult RemoveCategory([FromBody]Product product, [FromBody]Category category)
        //{
        //    if (product == null || category == null) return BadRequest();

        //    if (_productService.RemoveCategory(product, category))
        //    {
        //        return GetProductByName(product.Name);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        /// <summary>
        /// Hides product for display in product lists.
        /// </summary>
        /// <param name="product">The specific product to hide.</param>
        /// <returns>Represents whether the hiding is successful or not.</returns>
        //[SwaggerResponseRemoveDefaults]
        //[SwaggerResponse(HttpStatusCode.OK, "Hide result", typeof(Product))]
        //[SwaggerResponse(HttpStatusCode.NotFound, "Product not found in the database")]
        //[SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect product")]
        //[HttpPatch, Route("hide")]
        //public IHttpActionResult Hide([FromBody]Product product)
        //{
        //    if (product == null) return BadRequest();

        //    if (_productService.Hide(product))
        //    {
        //        return GetProductByName(product.Name);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        /// <summary>
        /// Reveal product for display in product lists.
        /// </summary>
        /// <param name="product">The specific product to reveal.</param>
        /// <returns>Represents whether the revealing is successful or not.</returns>
        //[SwaggerResponseRemoveDefaults]
        //[SwaggerResponse(HttpStatusCode.OK, "Reveal result", typeof(Product))]
        //[SwaggerResponse(HttpStatusCode.NotFound, "Product not found in the database")]
        //[SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect product")]
        //[HttpPatch, Route("reveal")]
        //public IHttpActionResult Reveal([FromBody]Product product)
        //{
        //    if (product == null) return BadRequest();

        //    if (_productService.Reveal(product))
        //    {
        //        return GetProductByName(product.Name);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        /// <summary>
        /// Checks if the specific product is hidden.
        /// </summary>
        /// <param name="product">The specific product under checking.</param>
        /// <returns>Represents if the product is in the hidden or revealed state.</returns>
        //[SwaggerResponseRemoveDefaults]
        //[SwaggerResponse(HttpStatusCode.OK, "IsHidden result")]
        //[SwaggerResponse(HttpStatusCode.NotFound, "Product not found in the database")]
        //[SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect product")]
        //[HttpGet, Route("is-hidden")]
        //public IHttpActionResult IsHidden([FromBody]Product product)
        //{
        //    if (product == null) return BadRequest();

        //    var result = _productService.IsHidden(product);
        //    if (result == null) return NotFound();

        //    if (result == true) return Content(HttpStatusCode.OK, "Product is hidden");
        //    else return Content(HttpStatusCode.OK, "Product is revealed");
        //}
    }
}
