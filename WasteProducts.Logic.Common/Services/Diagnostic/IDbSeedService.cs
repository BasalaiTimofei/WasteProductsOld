using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    /// <summary>
    /// Service, that seeds database with permanent and test entries.
    /// </summary>
    public interface IDbSeedService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="useTestData">If <c>true</c> database will seeded with test data.</param>
        /// <returns>Task</returns>
        Task SeedAsync(bool useTestData = false);
    }
}