using System;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories.Diagnostic
{
    /// <summary>
    /// Repository created for seeding purposes.
    /// </summary>
    public interface ISeedRepository : IDisposable
    {
        /// <summary>
        /// Seeds database with test data.
        /// </summary>
        /// <returns></returns>
        Task RecreateAndSeedAsync();
    }
}
