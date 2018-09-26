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
        /// Tries to add a new category by name and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="name">The name of the category to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        Task<string> Add(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameRange"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> AddRange(IEnumerable<string> nameRange);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetAll();
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category> GetById(string id);

        /// <summary>
        /// Returns a spicific category by its name.
        /// </summary>
        /// <param name="name">The name of the category to be gotten.</param>
        /// <returns>The specific category to be returned.</returns>
        Task<Category> GetByName(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task Update(Category category);

        /// <summary>
        /// Tries to delete the specific category.
        /// </summary>
        /// <param name="name">The name of the category to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        Task Delete(string id);
    }
}
