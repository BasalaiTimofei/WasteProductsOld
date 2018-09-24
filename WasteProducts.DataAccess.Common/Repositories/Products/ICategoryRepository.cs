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
        /// Adds a new category.
        /// </summary>
        /// <param name="category">The specific category for adding.</param>
        /// <returns>A task that represents the asynchronous add operation.
        /// The task result contains the id of the category written to the database.</returns>
        Task<string> AddAsync(CategoryDB category);

        /// <summary>
        /// Adds a collection of categories.
        /// </summary>
        /// <param name="categories">The specific collection of categories for adding.</param>
        /// <returns>A task that represents the asynchronous add operation.
        /// The task result contains the collection of ids of the category written to the database.</returns>
        Task<IEnumerable<string>> AddRangeAsync(IEnumerable<CategoryDB> categories);

        /// <summary>
        /// Deletes the specific category.
        /// </summary>
        /// <param name="category">The specific category for deleting.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        Task DeleteAsync(CategoryDB category);

        /// <summary>
        /// Deletes the specific category by id
        /// </summary>
        /// <param name="id">Represents a specific category id to delete.</param>
        /// <summary>
        /// Deletes the specific category by its id.
        /// </summary>
        /// <param name="id">Represents the id of the specific category to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        Task DeleteAsync(string id);

        /// <summary>
        /// Provides a collection of all categories.
        /// </summary>
        /// <returns>A task that represents the asynchronous select operation.
        /// The task result contains the collection of all categories from the database.</returns>
        Task<IEnumerable<CategoryDB>> SelectAllAsync();

        /// <summary>
        /// Provides a collection of categories that satisfy the condition.
        /// </summary>
        /// <param name="predicate">The condition that collection of categories must satisfy</param>
        /// <returns>A task that represents the asynchronous select operation.
        /// The task result contains the collection of categories from the database that satisfy a condition.</returns>
        Task<IEnumerable<CategoryDB>> SelectWhereAsync(Predicate<CategoryDB> predicate);

        /// <summary>
        /// Gets a category by its ID.
        /// </summary>
        /// <param name="id">The specific id of the category.</param>
        /// <returns>A task that represents the asynchronous get operation.
        /// The task result contains the category from the database.</returns>
        Task<CategoryDB> GetByIdAsync(string id);

        /// <summary>
        /// Gets a category by its name of the specific category.
        /// </summary>
        /// <param name="name">The name of the specific category.</param>
        /// <returns>A task that represents the asynchronous get operation.
        /// The task result contains the category from the database.</returns>
        Task<CategoryDB> GetByNameAsync(string name);

        /// <summary>
        /// Updates the specific category
        /// </summary>
        /// <param name="category">The specific category for updating</param>

        /// <summary>
        /// Updates the specific category.
        /// </summary>
        /// <param name="category">The specific category for updating.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        Task UpdateAsync(CategoryDB category);
    }
}
