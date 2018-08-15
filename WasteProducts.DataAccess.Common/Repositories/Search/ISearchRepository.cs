using System.Collections.Generic;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories.Search
{
    /// <summary>
    /// This interface provides CRUD methods for search repository
    /// </summary>
    public interface ISearchRepository
    {
        /// <summary>
        /// Returns object by Id
        /// </summary>
        /// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="keyValue">Key value of model key field</param>
        /// <param name="keyField">Key field of model</param>
        /// <returns>Object model</returns>
        TEntity Get<TEntity>(string keyValue, string keyField = "Id") where TEntity : class;

        /// <summary>
        /// Async version of Get
        /// </summary>
        Task<TEntity> GetAsync<TEntity>(string Id);

        /// <summary>
        /// Returns collection of all objects
        /// </summary>
        /// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="numResults">maximum number of resulte</param>
        /// <returns>IEnumerable of objects</returns>        
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;

        /// <summary>
        /// Returns collection of all objects. Query string and array of fields are used for search.
        /// </summary>
        /// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="queryString">Query string</param>
        /// <param name="searchableFileds">Fileds to search.</param>
        /// <param name="numResults">maximum number of results</param>
        /// <returns>IEnumerable of objects</returns>  
        IEnumerable<TEntity> GetAll<TEntity>(string queryString, string[] searchableFileds, int numResults) where TEntity : class;

        /// <summary>
        /// Async version of GetAll
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();

        /// <summary>
        /// Insert object to lucene object storage
        /// </summary>
        /// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="obj">Object</param>
        void Insert<TEntity>(TEntity obj) where TEntity : class;

        /// <summary>
        /// Async version of Insert
        /// </summary>
        Task InsertAsync<TEntity>(TEntity obj);

        /// <summary>
        /// Update object in lucene object storage
        /// </summary>
        /// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="obj">Object</param>
        void Update<TEntity>(TEntity obj) where TEntity : class;

        /// <summary>
        /// Async version of Update
        /// </summary>
        Task UpdateAsync<TEntity>(TEntity obj);

        /// <summary>
        /// Delete object from lucene object storage
        /// </summary>
        /// <typeparam name="TEntity">Object model type</typeparam>
        /// <param name="obj">Object</param>
        void Delete<TEntity>(TEntity obj) where TEntity : class;

        /// <summary>
        /// Async version of Delete
        /// </summary>
        Task DeleteAsync<TEntity>(TEntity obj);

        /// <summary>
        /// Clears repository
        /// </summary>
        void Clear();

        /// <summary>
        /// Optimizes repository for faster search
        /// </summary>
        void Optimize();

    }
}
