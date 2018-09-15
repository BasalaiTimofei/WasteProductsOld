using System;
using WasteProducts.DataAccess.Common.Context;

namespace WasteProducts.DataAccess.Contexts
{
    public class Database : IDatabase
    {
        private readonly WasteContext _dbContext;

        private bool _disposed;

        /// <inheritdoc />
        public Database(WasteContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public bool IsExists => _dbContext.Database.Exists();

        /// <inheritdoc />
        public bool IsCompatibleWithModel => _dbContext.Database.CompatibleWithModel(false);

        /// <inheritdoc />
        public void Initialize()
        {
            _dbContext.Database.Initialize(false);
        }

        /// <inheritdoc />
        public void Delete()
        {
            _dbContext.Database.Delete();
        }


        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                
                _disposed = true;
            }
        }

        ~Database()
        {
            Dispose(false);
        }
    }
}