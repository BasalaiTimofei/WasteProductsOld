using System;
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
    public class DbManagementService : IDbManagementService
    {
        private const string CreateSuccessMsg = "Database was successfully created.";
        private const string CreateFailMsg = "Database is already exist.";

        private const string SeedSuccessMsg = "Database was successfully seeded.";
        private const string SeedFailMsg = "Database was'n seeded because it don't exist or was exceptions in seeding process.";

        private const string DeleteSuccessMsg = "Database was successfully deleted.";
        private const string DeleteFailMsg = "Database isn't exist or already deleted.";

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
        public async Task<DatabaseResult> CreateAsync()
        {
            StringBuilder logBuilder = new StringBuilder();
            bool isSuccess = await ProcessDatabaseCommand(() => Task.FromResult(_dbContext.Database.CreateIfNotExists()), logBuilder);

            logBuilder.AppendLine(isSuccess ? CreateSuccessMsg : CreateFailMsg);
            _logger.Info(isSuccess ? CreateSuccessMsg : CreateFailMsg);

            return new DatabaseResult(isSuccess, logBuilder.ToString());
        }

        /// <inheritdoc />
        public async Task<DatabaseResult> DeleteAsync()
        {
            StringBuilder logBuilder = new StringBuilder();
            bool isSuccess = await ProcessDatabaseCommand(() => Task.FromResult(_dbContext.Database.Delete()), logBuilder);

            logBuilder.AppendLine(isSuccess ? DeleteSuccessMsg : DeleteFailMsg);
            _logger.Info(isSuccess ? DeleteSuccessMsg : DeleteFailMsg);

            return new DatabaseResult(isSuccess, logBuilder.ToString());
        }

        /// <inheritdoc />
        public async Task<DatabaseResult> SeedAsync(bool seedTestData)
        {
            StringBuilder logBuilder = new StringBuilder();
            bool isSuccess = await ProcessDatabaseCommand(async () => await _dbSeedService.SeedAsync(seedTestData), logBuilder);

            logBuilder.AppendLine(isSuccess ? SeedSuccessMsg : SeedFailMsg);
            _logger.Info(isSuccess ? SeedSuccessMsg : SeedFailMsg);

            return new DatabaseResult(isSuccess, logBuilder.ToString());
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
        /// <param name="func">Func with Database</param>
        /// <param name="logBuilder">StringBuilder for log</param>
        /// <returns></returns>
        private async Task<bool> ProcessDatabaseCommand(Func<Task<bool>> func, StringBuilder logBuilder)
        {
            bool isSuccess = false;

            var logAction = _dbContext.Database.Log; //wrap log action
            _dbContext.Database.Log = logMsg =>
            {
                logAction(logMsg);
                logBuilder.AppendLine(logMsg);
            };

            try
            {
                isSuccess = await func.Invoke();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    logBuilder.AppendLine( $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    
                    foreach (var ve in eve.ValidationErrors)
                    {
                        logBuilder.AppendLine($"- Property: \"{ve.PropertyName}\", Value: \"{eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName)}\", Error: \"{ve.ErrorMessage}\"");
                    }
                    logBuilder.AppendLine();
                }
                _logger.Error(e);
            }
            catch (SqlException e)
            {
                logBuilder.AppendLine(e.ToString());
                _logger.Error(e);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }

            _dbContext.Database.Log = logAction; //unwrap log action

            return isSuccess;
        }

        #endregion
    }
}