using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupDB
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public virtual string Information { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string AdminId { get; set; }

        /// <summary>
        /// User which created group
        /// </summary>
        public virtual UserDB Admin { get; set; }

        /// <summary>
        /// Users which as part of group
        /// </summary>
        public virtual IList<GroupUserDB> GroupUsers { get; set; }

        /// <summary>
        /// Boards with products
        /// </summary>
        public virtual IList<GroupBoardDB> GroupBoards { get; set; }

        /// <summary>
        /// true - group created;
        /// false - group deleted
        /// </summary>
        public virtual bool IsNotDeleted { get; set; }

        /// <summary>
        /// Group creation time
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Group delete time
        /// </summary>
        public virtual DateTime? Deleted { get; set; }

        /// <summary>
        /// Model modification time
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}