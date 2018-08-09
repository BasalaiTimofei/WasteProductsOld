using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models;
using WasteProducts.Logic.Common.Models.Search;

namespace WasteProducts.Logic.Common.Services.Search
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
        SearchResult Search<TEntity>(SearchQuery query);
		
		/// <summary>
        /// Async version of Search
        /// </summary>
        Task<SearchResult> SearchAsync<TEntity>(SearchQuery query);

        /// <summary>
        /// This method provides ability to search without specifying type of model
        /// </summary>
        /// <param name="query">SearchQuery model</param>
        /// <returns>SearchResult model</returns>
		SearchResult SearchAll(SearchQuery query);
		
		/// <summary>
        /// Async version of SearchAll
        /// </summary>
        Task<SearchResult> SearchAllAsync(SearchQuery query);


        /// <summary>
        /// This method provides ability to search without any formatting at search requests
        /// </summary>
        /// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="query">SearchQuery model</param>
        /// <returns>SearchResult model</returns>
        SearchResult SearchDefault<TEntity>(SearchQuery query);
		
		/// <summary>
        ///  Async version of SearchDefault
        /// </summary>
        Task<SearchResult> SearchDefaultAsync<TEntity>(SearchQuery query);

        /// <summary>
        /// This method provides ability to perform full-text search
        /// </summary>
        /// <param name="query">SearchQuery model</param>
        /// <returns>SearchResult model</returns>
		SearchResult SearchAllDefault(SearchQuery query);
		
		/// <summary>
        /// Async version of SearchAllDefault
        /// </summary>
        Task<SearchResult> SearchAllDefaultAsync(SearchQuery query);

        /// <summary>
        /// This method provides ability to add object to search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">Object model</param>
		void AddToSearchIndex<TEntity>(TEntity model);
		
        /// <summary>
        /// This method provides ability to add list of objects to search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">List of object models</param>
        void AddToSearchIndex<TEntity>(IEnumerable<TEntity> model);

        /// <summary>
        /// This method provides ability to remove object from search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">Object model</param>
		void RemoveSearchIndex<TEntity>(TEntity model);
		
		/// <summary>
        /// This method provides ability to remove list of objects from search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">List of object models</param>
        void RemoveSearchIndex<TEntity>(IEnumerable<TEntity> model);

        /// <summary>
        /// This method provides ability to update object in search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">Object model</param>		
        void UpdateInSearchIndex<TEntity>(TEntity model);
		
		/// <summary>
        /// This method provides ability to update list of objects in search repository
        /// </summary>
		/// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="model">List of object models</param>
        void UpdateInSearchIndex<TEntity>(IEnumerable<TEntity> model);

        /// <summary>
        /// This method provides ability to clear search repository storage
        /// </summary>
		bool ClearSearchIndex();
		
		/// <summary>
        /// This method provides ability to optimize search repository storage
        /// </summary>
        bool OptimizeSearchIndex();
    }
}
