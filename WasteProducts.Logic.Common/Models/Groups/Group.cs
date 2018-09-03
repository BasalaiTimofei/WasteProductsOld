using System.Collections.Generic;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Models.Groups
{
    public class Group
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public virtual string Information { get; set; }

        /// <summary>
        /// Boards with products
        /// </summary>
        public virtual IList<GroupBoard> GroupBoards { get; set; }
    }
}
