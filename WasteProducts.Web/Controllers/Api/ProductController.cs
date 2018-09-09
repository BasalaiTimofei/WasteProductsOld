using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Web.Controllers.Api
{
    [RoutePrefix("api/product")]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(ILogger logger, IProductService productService) : base(logger)
        {
            _productService = productService;
        }
        
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

        [HttpPost]
        [Route("add-product")]
        public IHttpActionResult AddProduct([FromBody]string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            if (_productService.AddByName(name))
            {
                return Content(HttpStatusCode.Created, "Product was successfully added");
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, "Product with specified name already exists");
            }
        }

        [HttpPost]
        [Route("add-product")]
        public IHttpActionResult AddProduct([FromBody]Barcode barcode)
        {
            if (barcode == null) return BadRequest();

            if (_productService.AddByBarcode(barcode))
            {
                return Content(HttpStatusCode.Created, "Product was successfully added");
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, "Product with specified barcode already exists");
            }
        }

        [HttpDelete]
        [Route("delete-product")]
        public IHttpActionResult DeleteProduct([FromBody]string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            if (_productService.DeleteByName(name))
            {
                return Content(HttpStatusCode.OK, "Product was successfully deleted");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("delete-product")]
        public IHttpActionResult DeleteProduct([FromBody]Barcode barcode)
        {
            if (barcode == null) return BadRequest();

            if (_productService.DeleteByBarcode(barcode))
            {
                return Content(HttpStatusCode.OK, "Product was successfully deleted");
            }
            else
            {
                return NotFound();
            }
        }

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