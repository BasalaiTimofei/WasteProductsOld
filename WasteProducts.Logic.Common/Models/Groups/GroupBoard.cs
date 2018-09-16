using System;
using System.Collections.Generic;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Models.Groups
{
    public class GroupBoard
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Board name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public virtual string Information { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string GroupId { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string CreatorId { get; set; }

        /// <summary>
        /// Products which add at board
        /// </summary>
        public virtual IList<GroupProduct> GroupProducts { get; set; }

        /// <summary>
        /// Messages sent by users
        /// </summary>
        public virtual IList<GroupComment> GroupProductComments { get; set; }
    }
}
