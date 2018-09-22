using System;
using System.Collections.Generic;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// Board administration service
    /// </summary>
    public interface IGroupBoardService
    {
        /// <summary>
        /// Create new board
        /// </summary>
        /// <param name="item">Object</param>
        string Create(GroupBoard item);

        /// <summary>
        /// Add or corect information on board
        /// </summary>
        /// <param name="item">Object</param>
        void Update(GroupBoard item);

        /// <summary>
        /// Board delete
        /// </summary>
        /// <param name="item">Object</param>
        void Delete(GroupBoard item);

        /// <summary>
        /// Search board by id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <return>Object</return>
        GroupBoard FindById(string id);
    }
}
