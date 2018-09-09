using System;
using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// User access service in group
    /// </summary>
    public interface IGroupUserService
    {
        void SendInvite<T>(T item, string adminId) where T : class;

        void DismissUser<T>(T item, string adminId) where T : class;

        void Enter<T>(T item) where T : class;

        void Leave<T>(T item) where T : class;

        IEnumerable<T> FindReceivedInvites<T>(string userId) where T : class;

        IEnumerable<T> FindUsersByGroupId<T>(Guid groupId) where T : class;
    }
}
