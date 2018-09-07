using System.Threading.Tasks;
using WasteProducts.Logic.Common.Attributes;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    /// <summary>
    /// Service, that seeds database with permanent and test entries.
    /// </summary>
    [Trace]
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