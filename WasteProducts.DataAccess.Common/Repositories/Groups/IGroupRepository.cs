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
    public interface IGroupRepository: IDisposable
    {
        /// <summary>
        /// Create - add a new object in db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">New object</param>
        void Create<T>(T item) where T : class;
        /// <summary>
        /// Update - correct object in db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">New object</param>
        void Update<T>(T item) where T : class;
        /// <summary>
        /// Update - correct object in db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">New objects</param>
        void Update<T>(IEnumerable<T> items) where T : class;
        /// <summary>
        /// Delete - delete object from db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">Primary key object</param>
        void Delete<T>(int id) where T : class;
        /// <summary>
        /// Get - getting object from db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">Primary key object</param>
        /// <returns>Object</returns>
        T Get<T>(int id) where T : class;
        /// <summary>
        /// GetAll - returns all objects
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> GetAll<T>() where T : class;
        /// <summary>
        /// Find - returns objects set with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="predicate">Lambda function</param>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> Find<T>(Func<T, Boolean> predicate) where T : class;
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="includeProperties">Expression trees</param>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> GetWithInclude<T>(params Expression<Func<T, 
            object>>[] includeProperties) where T : class;
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="predicate">Lambda function</param>
        /// <param name="includeProperties">Expression trees</param>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> GetWithInclude<T>(Func<T, bool> predicate,
            params Expression<Func<T, object>>[] includeProperties) where T : class;
        /// <summary>
        /// Save = save model 
        /// </summary>
        void Save();
    }
}
