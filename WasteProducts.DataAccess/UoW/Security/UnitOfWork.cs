using System.Data.Entity;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.UoW;
using WasteProducts.DataAccess.Repositories.Security;

namespace WasteProducts.DataAccess.UoW.Security
{
    /// <summary>
    /// UnitOfWork class has an inheritance from WasteProducts.DataAccess.Common.UoW.IUnitOfWork. 
    /// </summary>
    internal class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// ReadOnly DbFactory field will be set in constructor class
        /// </summary>
        private readonly DbFactory _dbFactory;

        /// <summary>
        /// DbContext for set Db field
        /// </summary>
        private DbContext _dbContext;

        //TODO replace invalid param @"constring"
        /// <summary>
        /// DbContext 
        /// </summary>
        private DbContext Db => _dbContext ?? (_dbContext = _dbFactory.Init(@"constring"));

        /// <summary>
        /// Initializes a new instance of UnitOfWork
        /// </summary>
        /// <param name="dbFactory">Used to set _dbContext when call _dbFactory.init() </param>
        public UnitOfWork(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// Apply change to DbContext
        /// </summary>
        public void SaveChanges()
        {
            Db.SaveChanges();
        }

        /// <summary>
        /// Apply change to DbContext async
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await Db.SaveChangesAsync();
        }
    }
}
