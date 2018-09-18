using System.Threading.Tasks;
using Moq;
using Ninject.Extensions.Logging;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Context;
using WasteProducts.DataAccess.Common.Repositories.Diagnostic;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Services;

namespace WasteProducts.Logic.Tests.Diagnostic_Tests
{
    [TestFixture]
    public class DbServiceTests
    {
        private const string IncorrectMethodWorkMsg = "Method works incorrect";
        private Mock<ILogger> _loggerMoq;
        private Mock<IDatabase> _databaseMoq;
        private Mock<IDbSeedService> _dbSeedServiceMoq;
        private Mock<ISeedRepository> _seedRepoMoq;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _loggerMoq = new Mock<ILogger>();
            _databaseMoq = new Mock<IDatabase>();
            _dbSeedServiceMoq = new Mock<IDbSeedService>();
            _seedRepoMoq = new Mock<ISeedRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _loggerMoq.Reset();
            _databaseMoq.Reset();
            _dbSeedServiceMoq.Reset();
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void GetStatus_Returns_DatabaseStatus_When(bool databaseIsExists, bool databaseIsCompatibleWithModel)
        {
            // arrange
            var dbManagementService = GetDbService();

            _databaseMoq.SetupGet(database => database.IsExists).Returns(databaseIsExists);
            _databaseMoq.SetupGet(database => database.IsCompatibleWithModel).Returns(databaseIsCompatibleWithModel);

            var expectedResult = new DatabaseState(databaseIsExists, databaseIsCompatibleWithModel);

            //action
            var actualResult = dbManagementService.GetStateAsync().Result;

            // assert
            Assert.AreEqual(expectedResult.IsExist, actualResult.IsExist, IncorrectMethodWorkMsg);
            Assert.AreEqual(expectedResult.IsCompatibleWithModel, actualResult.IsCompatibleWithModel, IncorrectMethodWorkMsg);

            _databaseMoq.VerifyGet(database => database.IsExists, Times.Once);

            if (databaseIsExists)
            {
                _databaseMoq.VerifyGet(database => database.IsCompatibleWithModel, Times.Once);

                if (!databaseIsCompatibleWithModel)
                    _loggerMoq.Verify(logger => logger.Warn(It.IsAny<string>()), Times.Once);
            }
            else
                _databaseMoq.VerifyGet(database => database.IsCompatibleWithModel, Times.Never);
        }

        [Test]
        public async Task DeleteAsync_Test()
        {
            // arrange
            var dbManagementService = GetDbService();

            // action
            await dbManagementService.DeleteAsync().ConfigureAwait(false);

            // assert
            _databaseMoq.Verify(database => database.Delete(), Times.Once);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ReCreateAsync_Test(bool seedTestData)
        {
            // arrange
            var dbManagementService = GetDbService();

            _databaseMoq.SetupGet(database => database.IsExists).Returns(false);

            // action
           await dbManagementService.ReCreateAsync(seedTestData).ConfigureAwait(false);

            // assert
            _databaseMoq.Verify(database => database.Delete(), Times.Once);
            _dbSeedServiceMoq.Verify(seedService => seedService.SeedBaseDataAsync(), Times.Once);

            if (seedTestData)
                _dbSeedServiceMoq.Verify(seedService => seedService.SeedTestDataAsync(), Times.Once);
            else
                _dbSeedServiceMoq.Verify(seedService => seedService.SeedTestDataAsync(), Times.Never);
        }

        IDbService GetDbService() => new DbService(_seedRepoMoq.Object, _dbSeedServiceMoq.Object, _databaseMoq.Object, _loggerMoq.Object);
    }
}
