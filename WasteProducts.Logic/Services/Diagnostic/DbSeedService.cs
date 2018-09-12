using System.Threading.Tasks;
using NLog;
using WasteProducts.Logic.Common.Factories;
using WasteProducts.Logic.Common.Services.Diagnostic;

namespace WasteProducts.Logic.Services
{
    /// <inheritdoc />
    public class DbSeedService : IDbSeedService
    {
        private readonly ITestModelsService _testModelsService;
        private readonly IServiceFactory _serviceFactory;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="testModelsService">Generates test models</param>
        /// <param name="serviceFactory">Service factory, for seeding in database through services.</param>
        /// <param name="logger">NLog logger</param>
        public DbSeedService(ITestModelsService testModelsService, IServiceFactory serviceFactory, ILogger logger)
        {
            _testModelsService = testModelsService;
            _serviceFactory = serviceFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task SeedBaseDataAsync()
        {
            _logger.Debug("Seeding base data");

            // TODO: Seeding using services from IServiceFactory
        }

        /// <inheritdoc />
        /// <summary>
        /// Seeds test data to database
        /// </summary>
        /// <returns>Task</returns>
        public async Task SeedTestDataAsync()
        {
            _logger.Debug("Seeding test data");

            // TODO: Seeding test data using services from IServiceFactory
        }
    }
}