using System;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using NLog;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Services.Diagnostic;

namespace WasteProducts.Logic.Services
{
    /// <inheritdoc />
    public class DbService : IDbService
    {
        private const string GetStatusDebugMsg = "Checking database status";
        private const string GetStatusWarnMsg = "Existing database don't compatible with model!";

        private const string DeleteDebugMsg = "Trying to delete database";
        private const string CreateAndSeedAsyncDebugMsg = "Trying to create and to seed in to database";


        private readonly IDbSeedService _dbSeedService;
        private readonly IDatabase _database;
        private readonly ILogger _logger;

        private bool _isDisposed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbSeedService">IDbSeedService implementation that seeds into database</param>
        /// <param name="database">IDatabase implementation, for operations with database</param>
        /// <param name="logger">NLog logger</param>
        public DbService(IDbSeedService dbSeedService, IDatabase database, ILogger logger)
        {
            _dbSeedService = dbSeedService;
            _database = database;
            _logger = logger;
        }

        ~DbService()
        {
            Dispose(false);
        }


        /// <inheritdoc />
        public DatabaseStatus GetStatus()
        {
            _logger.Debug(GetStatusDebugMsg);

            bool isExist = _database.IsExists;
            var isCompatibleWithModel = isExist && _database.IsCompatibleWithModel;

            if (isExist && !isCompatibleWithModel) _logger.Warn(GetStatusWarnMsg);

            return new DatabaseStatus(isExist, isCompatibleWithModel);
        }

        /// <inheritdoc />
        public bool Delete()
        {
            _logger.Debug(DeleteDebugMsg);

            if (!_database.IsExists)
                return false;

            return _database.Delete();
        }

        /// <inheritdoc />
        public async Task<bool> CreateAndSeedAsync(bool seedTestData)
        {
            _logger.Debug(CreateAndSeedAsyncDebugMsg);

            if (_database.IsExists)
                return false;

            _database.Initialize();
            await _dbSeedService.SeedBaseDataAsync().ConfigureAwait(false);

            if(seedTestData)
                await _dbSeedService.SeedTestDataAsync().ConfigureAwait(false);

            return true;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _database.Dispose();
                }
                
                _isDisposed = true;
            }
        }
    }
}