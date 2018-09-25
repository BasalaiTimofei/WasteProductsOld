using System;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Attributes;
using WasteProducts.Logic.Common.Models.Diagnostic;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    /// <summary>
    /// Service, that helps to create/seed/delete and gets status of the database;
    /// </summary>
    public interface IDbService : IDisposable
    {
        /// <summary>
        /// Returns state of database
        /// </summary>
        /// <returns>DatabaseState</returns>
        Task<DatabaseState> GetStateAsync();

        /// <summary>
        /// Deletes the database if it exists, otherwise do nothing .
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteAsync();

        /// <summary>
        /// Deletes database if it exists and create new.
        /// </summary>
        /// <param name="withTestData">If it set as <c>true</c>, the database will be created with the test data.</param>
        /// <returns>Task</returns>
        Task ReCreateAsync(bool withTestData = false);

        /// <summary>
        /// Recreates database and populates it with ISeedRepository.
        /// </summary>
        /// <returns>Task</returns>
        Task ReCreateAndSeedAsync();
    }
}