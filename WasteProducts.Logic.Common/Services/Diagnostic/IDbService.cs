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
        /// <returns>Task</returns>
        Task ReCreateAsync();

        /// <summary>
        /// Recreates database and populates it with ISeedRepository.
        /// </summary>
        /// <returns>Task</returns>
        Task SeedAsync();
    }
}