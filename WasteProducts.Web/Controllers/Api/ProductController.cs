using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using NLog;
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
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetProduct([FromBody]string name)
        {
            if (String.IsNullOrEmpty(name)) return BadRequest();

            Product product = _productService.GetByNameAsync(name).Result;
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
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetProducts([FromBody]Category category)
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
        /// <param name="name">The name of the product to be added.</param>
        /// <returns>Represents whether the addition is successful or not.</returns>
        [HttpPost]
        [Route("add-product")]
        public IHttpActionResult AddProduct([FromBody]string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            if (_productService.Add(name))
            {
                return Content(HttpStatusCode.Created, "Product was successfully added");
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, "Product with specified name already exists");
            }
        }

        /// <summary>
        /// Tries to add a new product by barcode and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="barcode">Barcode of the product to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        [HttpPost]
        [Route("add-product")]
        public IHttpActionResult AddProduct([FromBody]Barcode barcode)
        {
            if (barcode == null) return BadRequest();

            if (_productService.Add(barcode))
            {
                return Content(HttpStatusCode.Created, "Product was successfully added");
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, "Product with specified barcode already exists");
            }
        }

        /// <summary>
        /// Tries to delete the product by name and returns whether the deletion is successful or not.
        /// </summary>
        /// <param name="name">The name of the product to be deleted.</param>
        /// <returns>Represents whether the deletion is successful or not.</returns>
        [HttpDelete]
        [Route("delete-product")]
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
        [HttpDelete]
        [Route("delete-product")]
        public IHttpActionResult DeleteProduct([FromBody]Barcode barcode)
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
        [HttpPut]
        [Route("add-category")]
        public IHttpActionResult AddCategory([FromBody]Product product, [FromBody]Category category)
        {
            if (product == null || category == null) return BadRequest();

            if (_productService.AddCategory(product, category))
            {
                return GetProduct(product.Name);
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
        [HttpDelete]
        [Route("delete-category")]
        public IHttpActionResult RemoveCategory([FromBody]Product product, [FromBody]Category category)
        {
            if (product == null || category == null) return BadRequest();

            if (_productService.RemoveCategory(product, category))
            {
                return GetProduct(product.Name);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Hides product for display in product lists.
        /// </summary>
        /// <param name="product">The specific product to hide.</param>
        /// <returns>Represents whether the hiding is successful or not.</returns>
        [HttpPatch]
        [Route("hide")]
        public IHttpActionResult Hide([FromBody]Product product)
        {
            if (product == null) return BadRequest();

            if (_productService.Hide(product))
            {
                return Content(HttpStatusCode.OK, "Product is hidden for you now");
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Reveal product for display in product lists.
        /// </summary>
        /// <param name="product">The specific product to reveal.</param>
        /// <returns>Represents whether the revealing is successful or not.</returns>
        [HttpPatch]
        [Route("reveal")]
        public IHttpActionResult Reveal([FromBody]Product product)
        {
            if (product == null) return BadRequest();

            if (_productService.Reveal(product))
            {
                return Content(HttpStatusCode.OK, "Product is revealed for you now");
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Checks if the specific product is hidden.
        /// </summary>
        /// <param name="product">The specific product under checking.</param>
        /// <returns>Represents if the product is in the hidden or revealed state.</returns>
        [HttpGet]
        [Route("is-hidden")]
        public IHttpActionResult IsHidden([FromBody]Product product)
        {
            if (product == null) return BadRequest();

            var result = _productService.IsHidden(product);
            if (result == null) return NotFound();

            if (result == true) return Content(HttpStatusCode.OK, "Product is hidden");
            else return Content(HttpStatusCode.OK, "Product is revealed");
        }
    }
}