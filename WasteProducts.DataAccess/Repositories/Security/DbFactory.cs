using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Contexts.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    public class DbFactory
    {
        private IdentityContext _db;

        public IdentityContext Init(string nameOrConnectionString)
        {
            return _db ?? (_db = new IdentityContext(nameOrConnectionString));
        }

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }

                _disposed = true;
            }
        }
    }
}
