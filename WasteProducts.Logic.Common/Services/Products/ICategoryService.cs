using System;
using System.Collections.Generic;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Services.Products
{
    /// <summary>
    /// This interface provides category methods.
    /// </summary>
    public interface ICategoryService : IDisposable
    {
        /// <summary>
        /// Tries to add a new category by name and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="name">The name of the category to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        bool Add(string name);

        /// <summary>
        /// Tries to add a list of new categories by names and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="names">The list of names of the categories to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        bool AddRange(IEnumerable<string> names);

        /// <summary>
        /// Returns a spicific category by its name.
        /// </summary>
        /// <param name="name">The name of the category to be gotten.</param>
        /// <returns>The specific category to be returned.</returns>
        Category Get(string name);

        /// <summary>
        /// Adds the description for specific category.
        /// </summary>
        /// <param name="category">The specific category for which a description is added.</param>
        /// <param name="description">The specific description for the specfic category.</param>
        void SetDescription(Category category, string description);

        /// <summary>
        /// Tries to delete the specific category.
        /// </summary>
        /// <param name="name">The name of the category to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        bool Delete(string name);

        /// <summary>
        /// Tries to delete the list of specific categories.
        /// </summary>
        /// <param name="names">The list of names of the categories to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        bool DeleteRange(IEnumerable<string> names);
    }
}
