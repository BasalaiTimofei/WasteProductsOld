using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Services.Products
{
    /// <summary>
    /// This interface provides category methods.
    /// </summary>
    public interface ICategoryService : IDisposable
    {
        /// <summary>
        /// Tries to add a new category by name.
        /// </summary>
        /// <param name="name">The name of the category to be added.</param>
        /// <returns>A task that represents the add operation.
        /// The task result contains the id of the category written to the database.</returns>
        Task<string> Add(string name);

        /// <summary>
        /// Adds a collection of categories.
        /// </summary>
        /// <param name="nameRange">The specific collection of categories for adding.</param>
        /// <returns>A task that represents the add operation.
        /// The task result contains the collection of ids of the category written to the database.</returns>
        Task<IEnumerable<string>> AddRange(IEnumerable<string> nameRange);

        /// <summary>
        /// Provides a collection of all categories.
        /// </summary>
        /// <returns>A task that represents the get operation.
        /// The task result contains the collection of all categories from the database.</returns>
        Task<IEnumerable<Category>> GetAll();

        /// <summary>
        /// Provides a specific category by its id.
        /// </summary>
        /// <param name="id">The id of the specific category</param>
        /// <returns>A task that represents the get operation.
        /// The task result contains the category from the database.</returns>
        Task<Category> GetById(string id);

        /// <summary>
        /// Provides a specific category by its name.
        /// </summary>
        /// <param name="name">The name of the specific category</param>
        /// <returns>A task that represents the get operation.
        /// The task result contains the category from the database.</returns>
        Task<Category> GetByName(string name);

        /// <summary>
        ///  Updates the specific category.
        /// </summary>
        /// <param name="category">The specific category for updating.</param>
        /// <returns>A task that represents the update operation.</returns>
        Task Update(Category category);

        /// <summary>
        /// Deletes the specific category by its id.
        /// </summary>
        /// <param name="id">Represents the id of the specific category to delete.</param>
        /// <returns>A task that represents the delete operation.</returns>
        Task Delete(string id);
    }
}
