using System;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Diagnostic;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    /// <summary>
    /// Service, that helps to create/init/delete and fet status of the database;
    /// </summary>
    public interface IDbManagementService : IDisposable
    {
        /// <summary>
        /// Return state of database
        /// </summary>
        /// <returns>DatabaseState</returns>
        Task<DatabaseState> GetState();

        /// <summary>
        /// Creates a new database if database not exist and initialize with current initialization strategy, otherwise does nothing.
        /// </summary>
        /// <returns>DatabaseResult</returns>
        Task<DatabaseResult> CreateAndInitAsync(bool useTestData = false);

        /// <summary>
        /// Deletes the database if it exists, otherwise does nothing.
        /// </summary>
        /// <returns>DatabaseResult</returns>
        Task<DatabaseResult> DeleteAsync();
    }
}