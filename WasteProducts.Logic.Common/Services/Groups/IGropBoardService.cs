using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services
{
    /// <summary>
    /// Board administration service
    /// </summary>
    public interface IGropBoardService
    {
        /// <summary>
        /// Create - create new board
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Create<T>(T item);
        /// <summary>
        /// Update - add or corect information in board
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Update<T>(T item);
        /// <summary>
        /// Delete - delete board
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="item">Object</param>
        void Delete<T>(T item);
    }
}
