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
    [RoutePrefix("search/service")]
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
        public IEnumerable<Product> Get(string query)
        {
            IEnumerable<Product> searchResultList;

            SearchQuery searchQuery = new SearchQuery();
            searchQuery.AddField("Title").AddField("Description");

            searchQuery.Query = query;

            searchResultList = _searchService.Search<Product>(searchQuery);
            searchResultList = new List<Product>();
            //searchResultList.Add(new Product { Name = "aaaa", Description = "aaaaaaaaaa" });//для теста, что без DI все работает

            return searchResultList;
        }

        //[SwaggerResponse(HttpStatusCode.OK, "Get search result collection", typeof(IEnumerable<Product>))]
        //[SwaggerResponse(HttpStatusCode.NoContent, "Search result collection is empty", typeof(IEnumerable<Product>))]
        ////[Route("{fields:string[]}")]
        //public IEnumerable<Product> Get(string query, string[] fields)
        //{
        //    IEnumerable<Product> searchResultList;

        //    SearchQuery searchQuery = new SearchQuery();
        //    foreach (string filed in fields)
        //        searchQuery.AddField(filed);

        //    searchQuery.Query = query;

        //    searchResultList = _service.Search<Product>(searchQuery);

        //    return searchResultList;
        //}
    }
}
