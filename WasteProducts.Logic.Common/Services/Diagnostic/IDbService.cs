using System;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Attributes;
using WasteProducts.Logic.Common.Models.Diagnostic;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    /// <summary>
    /// Service, that helps to create/seed/delete and gets status of the database;
    /// </summary>
    [Trace]
    public interface IDbService : IDisposable
    {
        /// <summary>
        /// Return status of database
        /// </summary>
        /// <returns>DatabaseStatus</returns>
        DatabaseStatus GetStatus();

        /// <summary>
        /// Deletes the database if it exists, otherwise does nothing.
        /// </summary>
        /// <returns>"True" if database was successfully deleted or "False" if database don't exist.</returns>
        bool Delete();

        /// <summary>
        /// Creates a new database if database not exist and seed, otherwise does nothing.
        /// </summary>
        /// <param name="seedTestData">If <c>true</c> test data will be seeded too.</param>
        /// <returns>"True" if database was successfully created and seeded or "False" if database already exists.</returns>
        Task<bool> CreateAndSeedAsync(bool seedTestData = false);
    }
}