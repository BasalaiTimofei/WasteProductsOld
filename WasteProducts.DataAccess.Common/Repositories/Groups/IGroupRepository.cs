using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories.Groups
{
    /// <summary>
    /// Group repository
    /// </summary>
    public interface IGroupRepository : IDisposable
    {
        /// <summary>
        /// Add a new object in db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">New object</param>
        void Create<T>(T item) where T : class;

        /// <summary>
        /// Correct object in db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">New object</param>
        void Update<T>(T item) where T : class;

        /// <summary>
        /// Correct objects in db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">New objects</param>
        void UpdateAll<T>(IList<T> items) where T : class;

        /// <summary>
        /// Delete object from db by primary key
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">Primary key object</param>
        void Delete<T>(string id) where T : class;

        /// <summary>
        /// Delete object from db by object
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Delete<T>(T item) where T : class;

        /// <summary>
        /// Delete object from db by colection
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void DeleteAll<T>(IList<T> items) where T : class;

        /// <summary>
        /// Getting object from db
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">Primary key object</param>
        /// <returns>Object</returns>
        Task<T> Get<T>(string id) where T : class;

        /// <summary>
        /// Returns all objects
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <returns>IEnumerable objects</returns>
        Task<IEnumerable<T>> GetAll<T>() where T : class;

        /// <summary>
        /// Returns objects set with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="predicate">Lambda function</param>
        /// <returns>IEnumerable objects</returns>
        Task<IEnumerable<T>> Find<T>(Func<T, Boolean> predicate) where T : class;

        /// <summary>
        /// Immediate loading objects with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="includeProperties">Expression trees</param>
        /// <returns>IEnumerable objects</returns>
        Task<IEnumerable<T>> GetWithInclude<T>(params Expression<Func<T,
            object>>[] includeProperties) where T : class;

        /// <summary>
        /// Immediate loading objects with condition
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="predicate">Lambda function</param>
        /// <param name="includeProperties">Expression trees</param>
        /// <returns>IEnumerable objects</returns>
        Task<IEnumerable<T>> GetWithInclude<T>(Func<T, bool> predicate,
            params Expression<Func<T, object>>[] includeProperties) where T : class;

        /// <summary>
        /// Save model 
        /// </summary>
        Task Save();
    }
}
