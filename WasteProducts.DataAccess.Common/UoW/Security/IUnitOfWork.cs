using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.UoW.Security
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}