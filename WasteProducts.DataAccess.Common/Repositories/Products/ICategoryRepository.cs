using System;
using System.Collections.Generic;
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
        void Add(CategoryDB category);

        /// <summary>
        /// Deletes the specific category
        /// </summary>
        /// <param name="category">The specific category for deleting</param>
        void Delete(CategoryDB category);

        /// <summary>
        /// Deletes the specific category by id
        /// </summary>
        /// <param name="id">Represents a specific category id to delete</param>
        void Delete(int id);

        /// <summary>
        /// Provides a listing of all categories.
        /// </summary>
        /// <returns>Returns list of categories.</returns>
        IEnumerable<CategoryDB> SelectAll();

        /// <summary>
        /// Provides a listing of categories that satisfy the condition.
        /// </summary>
        /// <param name="predicate">The condition that list of categories must satisfy</param>
        /// <returns>Returns list of categories.</returns>
        IEnumerable<CategoryDB> SelectWhere(Predicate<CategoryDB> predicate);

        /// <summary>
        /// Gets category by ID
        /// </summary>
        /// <param name="id">The specific id of category that was sorted</param>
        /// <returns>Returns a category chosen by ID</returns>
        CategoryDB GetById(int id);

        /// <summary>
        /// Gets category by name of the specific category
        /// </summary>
        /// <param name="name">The name of the specific category</param>
        /// <returns>Returns a category chosen by its name</returns>
        CategoryDB GetByName(string name);

        /// <summary>
        /// Updates the specific category
        /// </summary>
        /// <param name="category">The specific category for updating</param>
        void Update(CategoryDB category);
    }
}
