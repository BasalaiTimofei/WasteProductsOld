using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        void Create(GroupComment item, string groupId);

        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="groupId">Primary key</param>
        void Update(GroupComment item, string groupId);

        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="groupId">Primary key</param>
        void Delete(GroupComment item, string groupId);

        /// <summary>
        /// Get comment by id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Object</returns>
        GroupComment FindById(string id);

        /// <summary>
        /// Get all comments  by boardId
        /// </summary>
        /// <param name="boardId">Primary key</param>
        /// <returns>IEnumerable<Object></returns>
        IEnumerable<GroupComment> FindtBoardComment(string boardId);
    }
}
