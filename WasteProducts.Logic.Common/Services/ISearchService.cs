using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models;

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
        IEnumerable<TEntity> Search<TEntity>(SearchQuery query) where TEntity : class;

        /// <summary>
        /// This method provides ability to search without any formatting at search requests
        /// </summary>
        /// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="query">SearchQuery model</param>
        /// <returns>SearchResult model</returns>
        IEnumerable<TEntity> SearchDefault<TEntity>(SearchQuery query);

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
        /// This method provides ability to optimize search repository storage for faster search
        /// </summary>
        void OptimizeSearchIndex();

        #region Async methods
        /// <summary>
        /// Async version of Search
        /// </summary>
        Task<IEnumerable<TEntity>> SearchAsync<TEntity>(SearchQuery query);

        /// <summary>
        ///  Async version of SearchDefault
        /// </summary>
        Task<IEnumerable<TEntity>> SearchDefaultAsync<TEntity>(SearchQuery query);

        #endregion
    }
}
