using NLog;
using Swagger.Net.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
        public const string DEFAULT_PRODUCT_COMPOSITION_FIELD = "Composition";
        public const string DEFAULT_PRODUCT_BARCODE_FIELD = "Barcode.Code";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchService">Search service</param>
        /// <param name="logger">NLog logger</param>
        public SearchController(ISearchService searchService, ILogger logger) : base(logger)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Performs full-text search by default fields "Name", "Composition", "Barcode".
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>Product collection</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetById search result collection", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect query string")]
        [HttpGet, Route("products/default")]
        public Task<IEnumerable<Product>> GetProductsDefaultFields([FromUri]string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            BoostedSearchQuery searchQuery = new BoostedSearchQuery();
            searchQuery.AddField(DEFAULT_PRODUCT_NAME_FIELD, 1.0f)
                .AddField(DEFAULT_PRODUCT_COMPOSITION_FIELD, 1.0f)
                .AddField(DEFAULT_PRODUCT_BARCODE_FIELD, 1.0f);
            searchQuery.Query = query;

            return Task.FromResult(_searchService.SearchProduct(searchQuery));
        }


        /// <summary>
        /// Performs full-text in specified fields.
        /// </summary>
        /// <param name="query">SearchQuery object converted from string "query;field1[:boost1],field2[:boost2],..."</param>
        /// <returns>Product collection</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetById search result collection", typeof(IEnumerable<Product>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect query string")]
        [HttpGet, Route("products/custom")]
        public Task<IEnumerable<Product>> GetProducts(BoostedSearchQuery query)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            return Task.FromResult(_searchService.SearchProduct(query));
        }
    }
}
