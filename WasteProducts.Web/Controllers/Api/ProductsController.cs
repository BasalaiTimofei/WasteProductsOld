using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ninject.Extensions.Logging;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Web.Controllers.Api
{
    [RoutePrefix("api/products")]
    public class ProductsController : BaseApiController
    {
        public ProductsController(ILogger logger) : base(logger)
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            // return all products
            return await Task.FromResult(Ok());
        }

        [HttpGet, Route("{id}")]
        public void GetById(string id)
        {

        }

        [HttpPost, Route("")]
        public void CreateProduct()
        {
            //HttpContext.Current.Request.Files
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