using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Contexts.Security;

namespace WasteProducts.DataAccess.Repositories.Security
{
    /// <summary>
    /// DbFactory class 
    /// </summary>
    public class DbFactory
    {
        /// <summary>
        /// _db field type of Security.IdentityContext
        /// </summary>
        private IdentityContext _db;

        /// <summary>
        /// Initializes a new instance of IdentityContext with connectionstring
        /// </summary>
        /// <param name="ConnectionString">connectionstring</param>
        public IdentityContext Init(string nameOrConnectionString)
        {
            return _db ?? (_db = new IdentityContext(nameOrConnectionString));
        }

        /// <summary>
        /// bool dispose variable
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Method for Execute Dispose() and do not call finializer after Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing">bool variable for dispose</param>
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
