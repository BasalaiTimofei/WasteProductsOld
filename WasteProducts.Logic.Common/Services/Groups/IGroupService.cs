using System;
using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// Group administration service
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Create new group
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Create<T>(T item) where T : class;

        /// <summary>
        /// Add or corect information in group
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Update<T>(T item) where T : class;

        /// <summary>
        /// Group delete
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Primary key</param>
        void Delete<T>(T item);

        /// <summary>
        /// Search group by id
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">primary key</param>
        T FindById<T>(Guid groupId) where T : class;

        /// <summary>
        /// Search group by name
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="id">Group name</param>
        T FindByAdmin<T>(string userId) where T : class;

        IEnumerable<T> GetIds<T>(string userId) where T : class;
    }
}
