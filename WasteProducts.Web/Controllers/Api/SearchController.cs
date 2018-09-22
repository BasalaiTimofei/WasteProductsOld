using Swagger.Net.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject.Extensions.Logging;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Search;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Controller that returns full-text search results from Lucene repository.
    /// </summary>
    [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect query string")]
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
        [HttpGet, Route("products/default")]
        public async Task<IEnumerable<Product>> GetProductsDefaultFields([FromUri]string query)
        {
            BoostedSearchQuery searchQuery = new BoostedSearchQuery();
            searchQuery.AddField(DEFAULT_PRODUCT_NAME_FIELD, 1.0f)
                .AddField(DEFAULT_PRODUCT_COMPOSITION_FIELD, 1.0f)
                .AddField(DEFAULT_PRODUCT_BARCODE_FIELD, 1.0f);
            searchQuery.Query = query;

            return await _searchService.SearchProductAsync(searchQuery);
        }


        /// <summary>
        /// Performs full-text in specified fields.
        /// </summary>
        /// <param name="query">SearchQuery object converted from string "query;field1[:boost1],field2[:boost2],..."</param>
        /// <returns>Product collection</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetById search result collection", typeof(IEnumerable<Product>))]
        [HttpGet, Route("products/custom")]
        public async Task<IEnumerable<Product>> GetProducts(BoostedSearchQuery query)
        {
            return await  _searchService.SearchProductAsync(query);
        }

        /// <summary>
        /// Returns previous user's queries similar to new query.
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>Product collection</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Get search result collection", typeof(IEnumerable<UserQuery>))]
        [HttpGet, Route("queries")]
        public async Task<IEnumerable<UserQuery>> GetUserQueries(string query)
        {
            return await _searchService.GetSimilarQueriesAsync(query);
        }
    }
}
