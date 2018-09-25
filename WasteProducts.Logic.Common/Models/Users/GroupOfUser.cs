namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// DAL level model of the group of a user.
    /// </summary>
    public class GroupOfUser
    {
        /// <summary>
        /// ID of the group.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if user can create boards;
        /// false - user can't create boards.
        /// </summary>
        public bool RightToCreateBoards { get; set; }
    }
}
