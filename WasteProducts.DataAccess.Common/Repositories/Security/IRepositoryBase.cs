using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    /// <summary>
    /// Interface IRepositoryBase for the IRepository. Has an inheritance from IRepository
    /// </summary>
    public interface IRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
       
    }
}