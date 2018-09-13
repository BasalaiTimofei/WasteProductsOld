using System;
using System.Collections.Generic;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// User access service in group
    /// </summary>
    public interface IGroupUserService
    {
        /// <summary>
        /// Send invite to the user
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        void SendInvite(GroupUser item, string adminId);

        /// <summary>
        /// Dismiss user from the group
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        void DismissUser(GroupUser item, string adminId);

        /// <summary>
        /// Join the group by invitation
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        void Enter(GroupUser item, string adminId);

        /// <summary>
        /// Leave from group
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        void Leave(GroupUser item, string adminId);

        /// <summary>
        /// Get entitle user
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        void GetEntitle(GroupUser item, string adminId);

        /// <summary>
        /// Get user invites
        /// </summary>
        /// <param name="userId">Primary key</param>
        /// <returns>IEnumerable<Object></returns>
        IEnumerable<GroupUser> FindReceivedInvites(string userId);

        /// <summary>
        /// Get all users in the group
        /// </summary>
        /// <param name="groupId">Primary key</param>
        /// <returns>IEnumerable<Object></returns>
        IEnumerable<GroupUser> FindUsersByGroupId(Guid groupId);

        /// <summary>
        /// Get the user group in which it is composed
        /// </summary>
        /// <param name="userId">Primary key</param>
        /// <returns>IEnumerable<Object></returns>
        IEnumerable<GroupUser> FindGroupsById(string userId);
    }
}
