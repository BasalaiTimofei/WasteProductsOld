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
    public class SearchController : BaseApiController
    {
        private ISearchService _searchService { get; }

        public SearchController(ILogger logger, ISearchService searchService) : base(logger)
        {
            _searchService = searchService;
        }

        
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get search result collection", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.NoContent, "Search result collection is empty", typeof(IEnumerable<Product>))]
        [HttpGet]
        public IEnumerable<Product> Product(string query)
        {
            SearchQuery searchQuery = new SearchQuery();
            searchQuery.AddField("Name").AddField("Description");
            searchQuery.Query = query;

            return _searchService.Search<Product>(searchQuery);
        }

        //[SwaggerResponse(HttpStatusCode.OK, "Get search result collection", typeof(IEnumerable<Product>))]
        //[SwaggerResponse(HttpStatusCode.NoContent, "Search result collection is empty", typeof(IEnumerable<Product>))]
        //[Route("{fields:string[]}")]
        //[HttpGet]
        //public IEnumerable<Product> Products(string query, string[] fields)
        //{
            IEnumerable<Product> searchResultList = new List<Product>();

        //    SearchQuery searchQuery = new SearchQuery();
        //    foreach (string filed in fields)
        //        searchQuery.AddField(filed);

        //    searchQuery.Query = query;

        //    searchResultList = _service.Search<Product>(searchQuery);

        //    return searchResultList;
        //}
    }
}
