using System;
using System.Threading.Tasks;
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
        Task Invite(GroupUser item);

        /// <summary>
        /// Kicks user from the group.
        /// </summary>
        /// <param name="item">Object</param>
        Task Kick(GroupUser item);

        /// <summary>
        /// Entitles user to create boards in the group.
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        Task GiveRightToCreateBoards(GroupUser item, string adminId);

        /// <summary>
        /// Takes away right to create boards from the user.
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="adminId">Primary key</param>
        Task TakeAwayRightToCreateBoards(GroupUser item, string adminId);
    }
}
