using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    /// <summary>
    /// Service, that seeds database with permanent and test entries.
    /// </summary>
    public interface IDbSeedService
    {
        /// <summary>
        /// Seeds to database through services.
        /// </summary>
        /// <param name="seedTestData">If <c>true</c> database will seeded with test data.</param>
        /// <returns>Return bool if seeding was finished successful</returns>
        Task<bool> SeedAsync(bool seedTestData = false);
    }
}