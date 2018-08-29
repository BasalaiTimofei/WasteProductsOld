using System.Data.Entity;
using System.Threading.Tasks;
using Moq;
using NLog;
using NUnit.Framework;

using WasteProducts.DataAccess.Common.Context;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Logic.Services;
using static NUnit.StaticExpect.Expectations;

namespace WasteProducts.Logic.Tests.DiagnosticTests
{
    [TestFixture()]
    public class DbManagementServiceTests
    {
        [Test]
        public async Task GetStatus_Always_Returns_DatabaseStatus()
        {
            var loggerMoq = CreateMoqLogger();
            var dbContextMoq = CreateMoqDbContext();
            var dbSeedServiceMoq = CreateMoqSeedService();

            var service = new DbManagementService(dbSeedServiceMoq.Object, dbContextMoq.Object, loggerMoq.Object);

            var actualDatabaseStatus = await service.GetStatus();

            Expect(actualDatabaseStatus, Is.Not.Null & Is.TypeOf<DatabaseStatus>());
        }

        [Test]
        public async Task GetStatus_Returns_DatabaseStatus_NotExist_And_NotCompatibleWithModel_When_Database_IsNotExist()
        {
            var loggerMoq = CreateMoqLogger();
            var dbContextMoq = CreateMoqDbContext();
            var dbSeedServiceMoq = CreateMoqSeedService();

            var service = new DbManagementService(dbSeedServiceMoq.Object, dbContextMoq.Object, loggerMoq.Object);
            var expectedDatabaseStatus = new DatabaseStatus()
            {
                IsDbExist = false,
                IsDbCompatibleWithModel = false
            };

            var actualDatabaseStatus = await service.GetStatus();

            Expect(actualDatabaseStatus, EqualTo(expectedDatabaseStatus));
        }

        private Mock<ILogger> CreateMoqLogger()
        {
            return new Mock<ILogger>();
        }

        private Mock<IDbContext> CreateMoqDbContext()
        {
            var ctxMoq = new Mock<IDbContext>() { DefaultValue = DefaultValue.Mock };

            var dbMoq = Mock.Get(ctxMoq.Object.Database);
            dbMoq.SetupProperty(database => database.Log, s => { });
            dbMoq.Setup(database => database.Exists()).Returns(false);
            dbMoq.Setup(database => database.CompatibleWithModel(It.IsAny<bool>())).Returns(false);

            return ctxMoq;
        }

        private Mock<IDbSeedService> CreateMoqSeedService()
        {
            return new Mock<IDbSeedService>();
        }
    }
}