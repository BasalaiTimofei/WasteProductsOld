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

        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetProducts(Category category)
        {
            IEnumerable<Product> products = _productService.GetByCategory(category);

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
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
                return GetProduct(product.Name);
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
                return GetProduct(product.Name);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("set-price")]
        public IHttpActionResult SetPrice(Product product, decimal price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_productService.SetPrice(product, price))
            {
                return GetProduct(product.Name);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("rate")]
        public IHttpActionResult Rate(Product product, int rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_productService.Rate(product, rating))
            {
                return GetProduct(product.Name);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("hide")]
        public IHttpActionResult Hide(Product product)
        {
            if (_productService.Hide(product))
            {
                return Content(HttpStatusCode.OK, "Product is hidden for you now");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("reveal")]
        public IHttpActionResult Reveal(Product product)
        {
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
        public IHttpActionResult IsHidden(Product product)
        {
            if (_productService.IsHidden(product))
            {
                return Content(HttpStatusCode.OK, "Your product is in the hidden state");
            }
            else
            {
                return NotFound();
            }
        }
    }
}