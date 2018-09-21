using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Search;

namespace WasteProducts.Logic.Common.Services
{
    /// <summary>
    /// This interface provides search methods
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// This method provides ability to search by partial words. It replaces all dashes "-" in search requests, and adds "*" (star) after each word.
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <param name="query">SearchQuery model</param>
        /// <returns>SearchResult model</returns>
        //IEnumerable<TEntity> Search<TEntity>(SearchQuery query) where TEntity : class;

        /// <summary>
        /// This method provides ability to search by partial words. It replaces all dashes "-" in search requests, and adds "*" (star) after each word.
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <param name="query">BoostedSearchQuery model</param>
        /// <returns>SearchResult model</returns>
        IEnumerable<TEntity> Search<TEntity>(BoostedSearchQuery query) where TEntity : class;

        /// <summary>
        /// This method provides ability to search without any formatting at search requests
        /// </summary>
        /// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="query">SearchQuery model</param>
        /// <returns>SearchResult model</returns>
        IEnumerable<TEntity> SearchDefault<TEntity>(BoostedSearchQuery query);

        /// <summary>
        /// This method provides ability to add object to search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">Object model</param>
		void AddToSearchIndex<TEntity>(TEntity model) where TEntity : class;
		
        /// <summary>
        /// This method provides ability to add list of objects to search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">List of object models</param>
        void AddToSearchIndex<TEntity>(IEnumerable<TEntity> model) where TEntity : class;

        /// <summary>
        /// This method provides ability to remove object from search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">Object model</param>
		void RemoveFromSearchIndex<TEntity>(TEntity model) where TEntity : class;
		
		/// <summary>
        /// This method provides ability to remove list of objects from search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">List of object models</param>
        void RemoveFromSearchIndex<TEntity>(IEnumerable<TEntity> model) where TEntity : class;

        /// <summary>
        /// This method provides ability to update object in search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">Object model</param>		
        void UpdateInSearchIndex<TEntity>(TEntity model) where TEntity : class;
		
		/// <summary>
        /// This method provides ability to update list of objects in search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">List of object models</param>
        void UpdateInSearchIndex<TEntity>(IEnumerable<TEntity> model) where TEntity : class;

        /// <summary>
        /// This method provides ability to clear search repository storage
        /// </summary>
		void ClearSearchIndex();

        /// <summary>
        /// Returns previous queries similar to query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<UserQuery>> GetSimilarQueriesAsync(string query);

        /// <summary>
        /// This method provides ability to optimize search repository storage for faster search
        /// </summary>
        void OptimizeSearchIndex();

        /// <summary>
        /// Return list of products by search query
        /// </summary>
        /// <param name="query">Search query</param>
        /// <returns>List of products</returns>
        IEnumerable<Product> SearchProduct(BoostedSearchQuery query);

        /// <summary>
        /// Async version of SearchProduct
        /// </summary>
        /// <param name="query">Search query</param>
        /// <returns>List of products</returns>
        Task<IEnumerable<Product>> SearchProductAsync(BoostedSearchQuery query);
     
    }
}
