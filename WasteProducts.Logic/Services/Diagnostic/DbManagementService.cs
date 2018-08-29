using System;
using System.Text;
using System.Threading.Tasks;
using NLog;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Models.Users;
using WasteProducts.Logic.Common.Services.Diagnostic;

namespace WasteProducts.Logic.Services
{
    /// <inheritdoc />
    public class DbManagementService : IDbManagementService
    {
        private readonly IDbSeedService _dbSeedService;
        private readonly IDbContext _dbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbSeedService">Service that seeding to database using services</param>
        /// <param name="dbContext">EF DbContext, for operations with database</param>
        /// <param name="logger">NLog logger</param>
        public DbManagementService(IDbSeedService dbSeedService, IDbContext dbContext, ILogger logger)
        {
            _dbSeedService = dbSeedService;
            _dbContext = dbContext;
            _logger = logger;
        }
        
        /// <inheritdoc />
        public async Task<DatabaseStatus> GetStatus()
        {
            bool isExist = _dbContext.Database.Exists();
            var isDbCompatibleWithModel = false;

            if (isExist)
            {
                isDbCompatibleWithModel = _dbContext.Database.CompatibleWithModel(false);
            }

            return await Task.FromResult(new DatabaseStatus()
            {
                IsDbExist = isExist,
                IsDbCompatibleWithModel = isDbCompatibleWithModel
            });
        }

        /// <inheritdoc />
        public async Task<DatabaseResult> CreateAndSeedAsync(bool useTestData = false)
        {
            bool isSuccess = false;
            string log = await CaptureLog( async () =>
            {
                isSuccess = _dbContext.Database.CreateIfNotExists();
                await _dbSeedService.SeedAsync(useTestData);
            });

            return new DatabaseResult(isSuccess, log);
        }

        /// <inheritdoc />
        public async Task<DatabaseResult> DeleteAsync()
        {
            bool isSuccess = false;
            string log = await CaptureLog(() => isSuccess = _dbContext.Database.Delete());

            return new DatabaseResult(isSuccess, log);
        }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Helps to get log of actions with database 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private async Task<string> CaptureLog(Action action)
        {
            //wrap log action
            var logBuilder = new StringBuilder();
            var logAction = _dbContext.Database.Log;
            _dbContext.Database.Log = logMsg =>
            {
                logAction(logMsg);
                logBuilder.AppendLine(logMsg);
            };

            await Task.Run(action);

            //unwrap log action
            _dbContext.Database.Log = logAction;

            return logBuilder.ToString();
        }

        #endregion

       
    }
}