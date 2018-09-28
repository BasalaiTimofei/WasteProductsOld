using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// Mofel of a friend from user's friendlist.
    /// </summary>
    public class Friend
    {
        /// <summary>
        /// Unique key for the friend.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique username of the friend.
        /// </summary>
        public string UserName { get; set; }
    }
}
