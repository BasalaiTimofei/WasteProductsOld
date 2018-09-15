using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    /// <summary>
    /// Service, that seeds database with permanent and test entries.
    /// </summary>
    public interface IDbSeedService
    {
        /// <summary>
        /// Seeds base data to database
        /// </summary>
        /// <returns>Task</returns>
        Task SeedBaseDataAsync();

        /// <summary>
        /// Seeds test data to database
        /// </summary>
        /// <returns>Task</returns>
        Task SeedTestDataAsync();
    }
}