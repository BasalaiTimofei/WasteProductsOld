using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// Board administration service
    /// </summary>
    public interface IGroupBoardService
    {
        /// <summary>
        /// Create new board
        /// </summary>
        /// <param name="item">Object</param>
        Task<string> Create(GroupBoard item);

        /// <summary>
        /// Add or corect information on board
        /// </summary>
        /// <param name="item">Object</param>
        Task Update(GroupBoard item);

        /// <summary>
        /// Board delete
        /// </summary>
        /// <param name="item">Primary key</param>
        Task Delete(string boardId);

        /// <summary>
        /// Search board by id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <return>Object</return>
        Task<GroupBoard> FindById(string id);
    }
}
