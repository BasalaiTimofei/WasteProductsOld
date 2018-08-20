using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.UoW;
using WasteProducts.DataAccess.Repositories.Security;

namespace WasteProducts.DataAccess.UoW.Security
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly DbFactory _dbFactory;
        private DbContext _dbContext;
        //TODO replace invalid param @"constring"
        private DbContext Db => _dbContext ?? (_dbContext = _dbFactory.Init(@"constring"));

        public UnitOfWork(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void SaveChanges()
        {
            Db.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Db.SaveChangesAsync();
        }
    }
}
