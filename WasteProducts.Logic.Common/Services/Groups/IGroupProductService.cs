using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services
{
    /// <summary>
    /// Product administration service
    /// </summary>
    public interface IGroupProductService
    {
        /// <summary>
        /// Create - create new product on board
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Create<T>(T item);
        /// <summary>
        /// Update - update product information on board
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Update<T>(T item);
        /// <summary>
        /// Delete - delete product from the board
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Delete<T>(int? id);
    }
}
