using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories.Diagnostic
{
    /// <summary>
    /// Repository created for seeding purposes.
    /// </summary>
    public interface IDiagnosticRepository : IDisposable
    {
        /// <summary>
        /// Recreates database.
        /// </summary>
        /// <returns></returns>
        Task RecreateAsync();

        /// <summary>
        /// Seeds database with test data.
        /// </summary>
        /// <param name="prodIds">Collection of pre-seeded real product IDs.</param>
        /// <returns></returns>
        Task SeedAsync(IList<string> prodIds);
    }
}
