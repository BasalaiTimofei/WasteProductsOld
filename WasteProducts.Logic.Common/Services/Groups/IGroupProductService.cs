using System;
using System.Collections.Generic;
using WasteProducts.Logic.Common.Models.Groups;

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
        /// <param name="item">Object</param>
        void Create(GroupProduct item, string userId, string groupId);

        /// <summary>
        /// Add or corect information on board
        /// </summary>
        /// <param name="item">Object</param>
        void Update(GroupProduct item, string userId, string groupId);

        /// <summary>
        /// Product delete
        /// </summary>
        /// <param name="item">Object</param>
        void Delete(GroupProduct item, string userId, string groupId);

        /// <summary>
        /// Search Product in board by id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <return>Object</return>
        GroupProduct FindById(string id);
    }
}
