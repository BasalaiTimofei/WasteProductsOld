using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    /// <summary>
    /// Represents many-to-many navigational table entity for Friends property in UserDB.
    /// </summary>
    public class Friend
    {
        public Friend()
        {
        }

        public Friend(string userId, string friendOfUserId)
        {
            UserId = userId;
            FriendOfUserId = friendOfUserId;
        }

        /// <summary>
        /// User id.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Id of a friend of a user.
        /// </summary>
        public virtual string FriendOfUserId { get; set; }
    }
}
