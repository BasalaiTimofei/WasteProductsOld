using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Common.Models.Groups
{
    public class GroupComment
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// This board
        /// </summary>
        public virtual GroupBoard GroupBoard { get; set; }

        /// <summary>
        /// User who send message
        /// </summary>
        public virtual User Commentator { get; set; }

        /// <summary>
        /// This comment
        /// </summary>
        public virtual string Comment { get; set; }
    }
}
