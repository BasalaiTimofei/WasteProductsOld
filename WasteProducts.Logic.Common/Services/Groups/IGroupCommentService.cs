using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// Service return information
    /// </summary>
    public interface IGroupCommentService
    {
        void Create<T>(T item, Guid groupId) where T : class;

        void Update<T>(T item, Guid groupId) where T : class;

        void Delete<T>(T item, Guid groupId) where T : class;

        T FindById<T>(Guid id) where T : class;

        IEnumerable<T> FindtBoardComment<T>(Guid boardId) where T : class;
    }
}
