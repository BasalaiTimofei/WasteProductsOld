using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories
{
    /// <summary>
    /// Group repository
    /// </summary>
    /// <typeparam name="T">Object</typeparam>
    public interface IGroupRepository<T> where T : class
    {
        /// <summary>
        /// Create - add a new object in db
        /// </summary>
        /// <param name="item">New object</param>
        void Create(T item);
        /// <summary>
        /// Update - correct object in db
        /// </summary>
        /// <param name="item">New object</param>
        void Update(T item);
        /// <summary>
        /// Delete - delete object from db
        /// </summary>
        /// <param name="id">Primary key object</param>
        void Delete(int id);
        /// <summary>
        /// Get - getting object from db
        /// </summary>
        /// <param name="id">Primary key object</param>
        /// <returns>Object</returns>
        T Get(int id);
        /// <summary>
        /// GetAll - returns all objects
        /// </summary>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Find - returns objects set with condition
        /// </summary>
        /// <param name="predicate">lambda function</param>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <param name="includeProperties">expression trees</param>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties);
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <param name="predicate">lambda function</param>
        /// <param name="includeProperties">expression trees</param>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> GetWithInclude(Func<T, bool> predicate,
            params Expression<Func<T, object>>[] includeProperties);
    }
}
