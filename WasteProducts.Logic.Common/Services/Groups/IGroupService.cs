using System;
using System.Collections.Generic;
using WasteProducts.Logic.Common.Models.Groups;

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
        /// <param name="item">Object</param>
        void Create(Group item);

        /// <summary>
        /// Add or corect information in group
        /// </summary>
        /// <param name="item">Object</param>
        void Update(Group item);

        /// <summary>
        /// Group delete
        /// </summary>
        /// <param name="item">Primary key</param>
        void Delete(Group item);

        /// <summary>
        /// Search group by id
        /// </summary>
        /// <param name="Id">Primary key</param>
        /// <returns>Object</returns>
        Group FindById(string Id);

        /// <summary>
        /// Search group by userId
        /// </summary>
        /// <param name="userId">Primary key</param>
        /// <returns>Object</returns>
        Group FindByAdmin(string userId);
    }
}
