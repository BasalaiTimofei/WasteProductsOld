using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services
{
    /// <summary>
    /// user access service in group
    /// </summary>
    public interface IGroupUserManagerService
    {
        /// <summary>
        /// Add - joining a group
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Add<T>(T item);
        /// <summary>
        /// Delete - leave a group
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Delete<T>(T item);
    }
}
