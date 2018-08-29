using NLog;
using Swagger.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Web.Controllers.Api
{
    [RoutePrefix("api/search/")]
    public class SearchController : BaseApiController
    {
        private ISearchService _searchService { get; }
        public const string DEFAULT_PRODUCT_NAME_FIELD = "Name";
        public const string DEFAULT_PRODUCT_DESCRIPTION_FIELD = "Description";

        public SearchController(ILogger logger, ISearchService searchService) : base(logger)
        {
            _searchService = searchService;
        }


        [HttpGet]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get search result collection", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.NoContent, "Search result collection is empty", typeof(IEnumerable<Product>))]
        //[Route("api/search/products/{query}")]
        public IEnumerable<Product> GetProductsDefaultFields(string query)
        {
            SearchQuery searchQuery = new SearchQuery();
            searchQuery.AddField(DEFAULT_PRODUCT_NAME_FIELD).AddField(DEFAULT_PRODUCT_DESCRIPTION_FIELD);
            searchQuery.Query = query;

            return _searchService.Search<Product>(searchQuery);
        }

        [HttpPost]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get search result collection", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.NoContent, "Search result collection is empty", typeof(IEnumerable<Product>))]
        //[Route("api/search/products/{query}")]
        public IEnumerable<Product> GetProducts(string query, string[] fields)
        {
            IEnumerable<Product> searchResultList = new List<Product>();

            SearchQuery searchQuery = new SearchQuery();
            searchQuery.Query = query;
            foreach (string filed in fields)
            {
                searchQuery.AddField(filed);
            }

            return _searchService.Search<Product>(searchQuery);
        }
    }
}
