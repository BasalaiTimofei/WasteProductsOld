using NLog;
using System;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Resources;

namespace WasteProducts.Logic.Services
{
    /// <inheritdoc />
    public class DbService : IDbService
    {
        private readonly IDbSeedService _dbSeedService;
        private readonly IDatabase _database;
        private readonly ILogger _logger;

        private bool _disposed = false;

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

        /// <inheritdoc />
        public async Task<DatabaseState> GetStateAsync()
        {
            bool isExist = await Task.FromResult(_database.IsExists).ConfigureAwait(false);
            bool isCompatibleWithModel = isExist && await Task.FromResult(_database.IsCompatibleWithModel).ConfigureAwait(false);

            if (isExist && !isCompatibleWithModel)
            {
                _logger.Warn(DbServiceResources.GetStatusAsync_WarnMsg);
            }

            return new DatabaseState(isExist, isCompatibleWithModel);
        }
        
        /// <inheritdoc />
        public Task DeleteAsync()
        {
            return Task.Run(()=> _database.Delete());
        }

        /// <inheritdoc />
        public async Task ReCreateAsync(bool withTestData)
        {
            await DeleteAsync().ConfigureAwait(false);
            await Task.Run(() => _database.Initialize()).ConfigureAwait(false);
            await _dbSeedService.SeedBaseDataAsync().ConfigureAwait(false);

            if(withTestData)
                await _dbSeedService.SeedTestDataAsync().ConfigureAwait(false);
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
                    _database.Dispose();
                }

                _disposed = true;
            }
        }

        ~DbService()
        {
            Dispose(false);
        }
    }
}