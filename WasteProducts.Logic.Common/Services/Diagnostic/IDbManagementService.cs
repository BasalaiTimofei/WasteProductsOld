using System;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Diagnostic;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    /// <summary>
    /// Service, that helps to create/seed/delete and gets status of the database;
    /// </summary>
    public interface IDbManagementService : IDisposable
    {
        /// <summary>
        /// Return status of database
        /// </summary>
        /// <returns>DatabaseStatus</returns>
        Task<DatabaseStatus> GetStatus();

        /// <summary>
        /// Creates a new database if database not exist and seed, otherwise does nothing.
        /// </summary>
        /// <returns>DatabaseResult</returns>
        Task<DatabaseResult> CreateAndSeedAsync(bool useTestData = false);

        /// <summary>
        /// Deletes the database if it exists, otherwise does nothing.
        /// </summary>
        /// <returns>DatabaseResult</returns>
        Task<DatabaseResult> DeleteAsync();
    }
}