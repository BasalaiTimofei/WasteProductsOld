using System;
using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// Product administration service
    /// </summary>
    public interface IGroupProductService
    {
        /// <summary>
        /// Create new board
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Create<T>(T item, string userId, Guid groupId) where T : class;

        /// <summary>
        /// Add or corect information on board
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Update<T>(T item, string userId, Guid groupId) where T : class;

        /// <summary>
        /// Product delete
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Delete<T>(T item, string userId, Guid groupId) where T : class;

        /// <summary>
        /// Search board by id
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">Primary key</param>
        T FindById<T>(Guid id) where T : class;
    }
}
