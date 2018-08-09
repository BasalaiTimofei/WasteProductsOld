using System.Collections.Generic;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories.Search
{
    /// <summary>
    /// This interface provides SearchRepository methods
    /// </summary>
    public interface ISearchRepository
    { 
        /// <summary>
        /// Returnes object by Id
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <param name="Id">Id of model</param>
        /// <returns>Object model</returns>
        TEntity Get<TEntity>(string Id);
        Task<TEntity> GetAsync<TEntity>(string Id);

        /// <summary>
        /// Returnes collection of all objects
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <returns>IEnumerable of objects</returns>        
        IEnumerable<TEntity> GetAll<TEntity>();
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();

        /// <summary>
        /// Insert object to lucene object storage
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <param name="obj">Object</param>
        void Insert<TEntity>(TEntity obj);
        Task InsertAsync<TEntity>(TEntity obj);

        /// <summary>
        /// Update object in lucene object storage
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <param name="obj">Object</param>
        void Update<TEntity>(TEntity obj);
        Task UpdateAsync<TEntity>(TEntity obj);

        /// <summary>
        /// Delete object from lucene object storage
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <param name="obj">Object</param>
        void Delete<TEntity>(TEntity obj);
        Task DeleteAsync<TEntity>(TEntity obj);        
    }
}
