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
        void Create<T>(T item, string userId) where T : class;

        void Update<T>(T item, string userId) where T : class;

        void Delete<T>(T item, string userId) where T : class;

        T FindById<T>(Guid id) where T : class;

        T FindAll<T>(Guid id) where T : class;
    }
}
