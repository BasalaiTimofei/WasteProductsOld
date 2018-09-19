using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Products;

namespace WasteProducts.DataAccess.Common.Repositories.Products
{
    /// <summary>
    /// Interface for the CategoryRepository. Has an inheritance branch from IDisposable.
    /// </summary>
    public interface ICategoryRepository : IDisposable
    {
        /// <summary>
        /// Adds a new category
        /// </summary>
        /// <param name="category">The specific category for adding</param>
        Task<int> AddAsync(CategoryDB category);

        /// <summary>
        /// Deletes the specific category
        /// </summary>
        /// <param name="category">The specific category for deleting</param>
        Task DeleteAsync(CategoryDB category);

        /// <summary>
        /// Deletes the specific category by id
        /// </summary>
        /// <param name="id">Represents a specific category id to delete</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Provides a listing of all categories.
        /// </summary>
        /// <returns>Returns list of categories.</returns>
        Task<IEnumerable<CategoryDB>> SelectAllAsync();

        /// <summary>
        /// Provides a listing of categories that satisfy the condition.
        /// </summary>
        /// <param name="predicate">The condition that list of categories must satisfy</param>
        /// <returns>Returns list of categories.</returns>
        Task<IEnumerable<CategoryDB>> SelectWhereAsync(Predicate<CategoryDB> predicate);

        /// <summary>
        /// Gets category by ID
        /// </summary>
        /// <param name="id">The specific id of category that was sorted</param>
        /// <returns>Returns a category chosen by ID</returns>
        Task<CategoryDB> GetByIdAsync(int id);

        /// <summary>
        /// Gets category by name of the specific category
        /// </summary>
        /// <param name="name">The name of the specific category</param>
        /// <returns>Returns a category chosen by its name</returns>
        Task<CategoryDB> GetByNameAsync(string name);

        /// <summary>
        /// Updates the specific category
        /// </summary>
        /// <param name="category">The specific category for updating</param>
        Task UpdateAsync(CategoryDB category);
    }
}
