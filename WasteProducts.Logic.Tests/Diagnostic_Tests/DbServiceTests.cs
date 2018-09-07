using System;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Moq;
using NLog;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Context;
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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _loggerMoq = new Mock<ILogger>();
            _databaseMoq = new Mock<IDatabase>();
            _dbSeedServiceMoq = new Mock<IDbSeedService>();
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

            var expectedResult = new DatabaseStatus(databaseIsExists, databaseIsCompatibleWithModel);

            //action
            var actualResult = dbManagementService.GetStatus();

            // assert
            Assert.AreEqual(expectedResult, actualResult, IncorrectMethodWorkMsg);
            _databaseMoq.VerifyGet(database => database.IsExists, Times.Once);

            if (databaseIsExists)
            {
                _databaseMoq.VerifyGet(database => database.IsCompatibleWithModel, Times.Once);

                if (!databaseIsCompatibleWithModel)
                    _loggerMoq.Verify(logger => logger.Warn(It.IsAny<string>()), Times.Once);
            }
            else
                _databaseMoq.VerifyGet(database => database.IsCompatibleWithModel, Times.Never);

            _loggerMoq.Verify(logger => logger.Debug(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Delete_Returns_False_When_Database_DoNotExist()
        {
            // arrange
            var dbManagementService = GetDbService();

            _databaseMoq.SetupGet(database => database.IsExists).Returns(false);

            // action
            var actualResult = dbManagementService.Delete();

            // assert
            Assert.IsFalse(actualResult, IncorrectMethodWorkMsg);

            _databaseMoq.VerifyGet(database => database.IsExists, Times.Once);
            _databaseMoq.Verify(database => database.Delete(), Times.Never);

            _loggerMoq.Verify(logger => logger.Debug(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Delete_Returns_True_When_Database_IsExist()
        {
            // arrange
            var dbManagementService = GetDbService();

            _databaseMoq.SetupGet(database => database.IsExists).Returns(true);
            _databaseMoq.Setup(database => database.Delete()).Returns(true);

            // action
            var actualResult = dbManagementService.Delete();

            // assert
            Assert.IsTrue(actualResult, IncorrectMethodWorkMsg);

            _databaseMoq.VerifyGet(database => database.IsExists, Times.Once);
            _databaseMoq.Verify(database => database.Delete(), Times.Once);

            _loggerMoq.Verify(logger => logger.Debug(It.IsAny<string>()), Times.Once);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task CreateAndSeedAsync_Returns_True_When_Database_DoNotExist(bool seedTestData)
        {
            // arrange
            var dbManagementService = GetDbService();

            _databaseMoq.SetupGet(database => database.IsExists).Returns(false);

            // action
            var actualResult = await dbManagementService.CreateAndSeedAsync(seedTestData);

            // assert
            Assert.IsTrue(actualResult, IncorrectMethodWorkMsg);

            _databaseMoq.VerifyGet(database => database.IsExists, Times.Once);

            _dbSeedServiceMoq.Verify(seedService => seedService.SeedBaseDataAsync(), Times.Once);

            if (seedTestData)
                _dbSeedServiceMoq.Verify(seedService => seedService.SeedTestDataAsync(), Times.Once);
            else
                _dbSeedServiceMoq.Verify(seedService => seedService.SeedTestDataAsync(), Times.Never);

            _loggerMoq.Verify(logger => logger.Debug(It.IsAny<string>()), Times.Once);
        }


        [TestCase(false)]
        [TestCase(true)]
        public async Task CreateAndSeedAsync_Returns_False_When_Database_IsExist(bool seedTestData)
        {
            // arrange
            var dbManagementService = GetDbService();

            _databaseMoq.SetupGet(database => database.IsExists).Returns(true);

            // action
            var actualResult = await dbManagementService.CreateAndSeedAsync(seedTestData);

            // assert
            Assert.IsFalse(actualResult, IncorrectMethodWorkMsg);
            _databaseMoq.VerifyGet(database => database.IsExists, Times.Once);

            _dbSeedServiceMoq.Verify(seedService => seedService.SeedBaseDataAsync(), Times.Never);
            _dbSeedServiceMoq.Verify(seedService => seedService.SeedTestDataAsync(), Times.Never);

            _loggerMoq.Verify(logger => logger.Debug(It.IsAny<string>()), Times.Once);
        }


        IDbService GetDbService() => new DbService(_dbSeedServiceMoq.Object, _databaseMoq.Object, _loggerMoq.Object);
    }
}
