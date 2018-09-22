using System;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// User access service in group
    /// </summary>
    public interface IGroupUserService : IDisposable
    {
        /// <summary>
        /// Sends invite to the user.
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        void Invite(GroupUser item, string adminId);

        /// <summary>
        /// Kicks user from the group.
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        void Kick(GroupUser item, string adminId);

        /// <summary>
        /// Entitles user to create boards in the group.
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        void GiveRightToCreateBoards(GroupUser item, string adminId);

        /// <summary>
        /// Takes away right to create boards from the user.
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        void TakeAwayRightToCreateBoards(GroupUser item, string adminId);
    }
}
