using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Common.Services.Groups
{
    /// <summary>
    /// Product administration service
    /// </summary>
    public interface IGroupProductService
    {
        /// <summary>
        /// Create new board
        /// </summary>
        /// <param name="item">Object</param>
        Task<string> Create(GroupProduct item);

        /// <summary>
        /// Add or corect information on board
        /// </summary>
        /// <param name="item">Object</param>
        Task Update(GroupProduct item);

        /// <summary>
        /// Product delete
        /// </summary>
        /// <param name="producId">Primary key</param>
        Task Delete(string groupProductId);

        /// <summary>
        /// Search Product in board by id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <return>Object</return>
        Task<GroupProduct> FindById(string groupProductId);
    }
}
