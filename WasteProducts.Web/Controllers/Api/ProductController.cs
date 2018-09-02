using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;
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
        

    }
}