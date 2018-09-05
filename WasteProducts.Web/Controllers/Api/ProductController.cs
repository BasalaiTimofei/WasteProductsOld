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
        public IHttpActionResult GetProduct(string name)
        {
            Product product = _productService.GetByNameAsync(name).Result;
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [Route("add-product")]
        public IHttpActionResult AddProduct(string name)
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
        public IHttpActionResult AddProduct(Barcode barcode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
        public IHttpActionResult DeleteProduct(string name)
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
        public IHttpActionResult DeleteProduct(Barcode barcode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
        public IHttpActionResult AddCategory(Product product, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_productService.AddCategory(product, category))
            {
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("delete-category")]
        public IHttpActionResult RemoveCategory(Product product, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_productService.RemoveCategory(product, category))
            {
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }
    }
}