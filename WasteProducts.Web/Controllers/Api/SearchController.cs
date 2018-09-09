using NLog;
using Swagger.Net.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Search;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Controller that returns full-text search results from Lucene repository.
    /// </summary>
    [RoutePrefix("api/search")]
    public class SearchController : BaseApiController
    {
        private ISearchService _searchService { get; }
        public const string DEFAULT_PRODUCT_NAME_FIELD = "Name";
        public const string DEFAULT_PRODUCT_DESCRIPTION_FIELD = "Description";
        public const string DEFAULT_PRODUCT_BARCODE_FIELD = "Barcode.Code";

        public SearchController(ILogger logger, ISearchService searchService) : base(logger)
        {
            _searchService = searchService;
        }


        /// <summary>
        /// Performs full-text search by default fields "Name", "Description", "Barcode".
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>Product collection</returns>
        [HttpGet]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get search result collection", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect query string")]
        //[SwaggerResponse(HttpStatusCode.NoContent, "Search result collection is empty")]
        [Route("products/default")]
        public async Task<IEnumerable<Product>> GetProductsDefaultFields([FromUri]string query)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }
            else
            {
                BoostedSearchQuery searchQuery = new BoostedSearchQuery();
                searchQuery.AddField(DEFAULT_PRODUCT_NAME_FIELD, 1.0f).AddField(DEFAULT_PRODUCT_DESCRIPTION_FIELD, 1.0f).AddField(DEFAULT_PRODUCT_BARCODE_FIELD, 1.0f);
                searchQuery.Query = query;

                //return _searchService.Search<Product>(searchQuery);
                return await Task.FromResult(_searchService.SearchProduct(searchQuery));
            }
        }


        /// <summary>
        /// Performs full-text in specified fields.
        /// </summary>
        /// <param name="query">SearchQuery object converted from string "query;field1[:boost1],field2[:boost2],..."</param>
        /// <returns>Product collection</returns>
        [HttpGet]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get search result collection", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect query string")]
        [Route("products/custom")]
        public async Task<IEnumerable<Product>> GetProducts(BoostedSearchQuery query)
        {

            HttpResponseMessage response;

            if (ModelState.IsValid)
            {
                return await Task.FromResult(_searchService.SearchProduct(query));
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }
        }
    }
}
