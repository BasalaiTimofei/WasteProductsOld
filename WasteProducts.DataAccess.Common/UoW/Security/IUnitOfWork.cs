using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.UoW.Security
{
    /// <summary>
    /// Interface IUnitOfWork for the Security UnitOfWork.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Apply change to DbContext
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Apply change to DbContext async
        /// </summary>
        Task SaveChangesAsync();
    }
}