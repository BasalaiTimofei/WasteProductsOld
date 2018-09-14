using System;
using System.Collections.Generic;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Models.Groups
{
    public class Group
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string AdminId { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public virtual string Information { get; set; }

        /// <summary>
        /// Boards with products
        /// </summary>
        public virtual IList<GroupBoard> GroupBoards { get; set; }

        /// <summary>
        /// Group users
        /// </summary>
        public virtual IList<GroupUser> GroupUsers { get; set; }
    }
}
