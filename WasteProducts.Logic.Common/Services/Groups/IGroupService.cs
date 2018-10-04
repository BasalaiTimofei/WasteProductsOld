using System;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// Group administration service
    /// </summary>
    public interface IGroupService : IDisposable
    {

        /// <summary>
        /// Create new group
        /// </summary>
        /// <param name="item">Object</param>
        Task<Group> Create(Group item);

        /// <summary>
        /// Add or corect information in group
        /// </summary>
        /// <param name="item">Object</param>
        Task Update(Group item);

        /// <summary>
        /// Group delete
        /// </summary>
        /// <param name="groupId">Primary key</param>
        Task Delete(string groupId);

        /// <summary>
        /// Search group by id
        /// </summary>
        /// <param name="Id">Primary key</param>
        /// <returns>Object</returns>
        Task<Group> FindById(string Id);

        /// <summary>
        /// Search group by userId
        /// </summary>
        /// <param name="userId">Primary key</param>
        /// <returns>Object</returns>
        Task<Group> FindByAdmin(string userId);

        /// <summary>
        /// Search group by its name
        /// </summary>
        /// <param name="name">Name of the group</param>
        /// <returns>Object</returns>
        Task<Group> FindByName(string name);
    }
}
