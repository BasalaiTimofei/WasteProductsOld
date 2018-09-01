using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WasteProducts.Logic.Common.Services
{
    /// <summary>
    /// Service return information
    /// </summary>
    public interface IGroupDataService
    {
        /// <summary>
        /// Get - return one object
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">Primary key</param>
        /// <returns>Object</returns>
        T Get<T>(int id);
        /// <summary>
        /// GetAll - return all objects
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <returns>Objects</returns>
        IEnumerable<T> GetAll<T>();
        /// <summary>
        /// Find - returns objects set with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="predicate">lambda function</param>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> Find<T>(Func<T, Boolean> predicate);
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="includeProperties">expression trees</param>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> GetWithInclude<T>(params Expression<Func<T, object>>[] includeProperties);
        /// <summary>
        /// GetWithInclude - immediate loading objects with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="predicate">lambda function</param>
        /// <param name="includeProperties">expression trees</param>
        /// <returns>IEnumerable objects</returns>
        IEnumerable<T> GetWithInclude<T>(Func<T, bool> predicate,
            params Expression<Func<T, object>>[] includeProperties);
    }
}
