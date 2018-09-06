using System;
using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// User access service in group
    /// </summary>
    public interface IGroupUserService
    {
        void SendInvite<T>(T item) where T : class;

        void DismissUser<T>(T item) where T : class;

        void EnteredUser<T>(T item) where T : class;

        IEnumerable<T> FindInvites<T>(string userId) where T : class;

        IEnumerable<T> FindSendInvites<T>(Guid id) where T : class;
    }
}
