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

        void Insert<TEntity>(TEntity obj);
        void Update<TEntity>(TEntity obj);
        void Delete<TEntity>(TEntity obj);
        //todo add async version of Insert, Update, Delete

        /// <summary>
        /// Returnes collection of all objects
        /// </summary>
        /// <typeparam name="TEntity">Object model</typeparam>
        /// <returns>IEnumerable of objects</returns>        
        IEnumerable<TEntity> GetAll<TEntity>();
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();
    }
}
