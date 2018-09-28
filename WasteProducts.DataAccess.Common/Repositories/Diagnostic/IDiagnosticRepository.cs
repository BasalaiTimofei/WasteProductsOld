using System;
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
        /// <returns></returns>
        Task SeedAsync();
    }
}
