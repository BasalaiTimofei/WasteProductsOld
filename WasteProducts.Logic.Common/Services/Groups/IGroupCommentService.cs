using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// Service comment
    /// </summary>
    public interface IGroupCommentService
    {
        /// <summary>
        /// Add new comment
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="groupId">Primary key</param>
        Task<string> Create(GroupComment item, string groupId);

        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="groupId">Primary key</param>
        Task Update(GroupComment item, string groupId);

        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="groupId">Primary key</param>
        Task Delete(GroupComment item, string groupId);

        /// <summary>
        /// Get comment by id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Object</returns>
        Task<GroupComment> FindById(string id);

        /// <summary>
        /// Get all comments  by boardId
        /// </summary>
        /// <param name="boardId">Primary key</param>
        /// <returns>IEnumerable<Object></returns>
        Task<IEnumerable<GroupComment>> FindtBoardComment(string boardId);
    }
}
