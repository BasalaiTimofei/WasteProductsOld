using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Services.Products
{
    /// <summary>
    /// This interface provides product methods.
    /// </summary>
    public interface IProductService : IDisposable
    {
        /// <summary>
        /// Tries to add a new product and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="product">The product to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        bool Add(Product product, out Product addedProduct);

        /// <summary>
        /// Tries to add a new product by barcode and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="barcode">Barcode of the product to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        bool Add(Barcode barcode, out Product addedProduct);

        /// <summary>
        /// Tries to add a new product by name and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="name">The name of the product to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        bool Add(string name, out Product addedProduct);

        /// <summary>
        /// Gets the product by its id.
        /// </summary>
        /// <param name="id">The id of the product.</param>
        /// <returns>The product with the specific id.</returns>
        Product GetById(string id);

        /// <summary>
        /// Gets the product by its barcode.
        /// </summary>
        /// <param name="barcode">The barcode of the product.</param>
        /// <returns>The product with the specific barcode.</returns>
        Product GetByBarcode(Barcode barcode);

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>All product in the application.</returns>
        IEnumerable<Product> GetAll();

        /// <summary>
        /// Gets product by its name.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <returns>Product with the specific name.</returns>
        Product GetByName(string name);

        /// <summary>
        /// Gets asynchronously product by its name.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <returns>Product with the specific name.</returns>
        Task<Product> GetByNameAsync(string name);

        /// <summary>
        /// Gets products by a category.
        /// </summary>
        /// <param name="category">Category of the product.</param>
        /// <returns>Product with the specific name.</returns>
        IEnumerable<Product> GetByCategory(Category category);

        /// <summary>
        /// Tries to delete the product by barcode and returns whether the deletion is successful or not.
        /// </summary>
        /// <param name="barcode">Barcode of the product to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        bool Delete(Barcode barcode);

        /// <summary>
        /// Tries to delete the product by name and returns whether the deletion is successful or not.
        /// </summary>
        /// <param name="name">The name of the product to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        bool Delete(string name);


        /// <summary>
        /// Deletes product from Database.
        /// </summary>
        /// <param name="product">Product that should be deleted.</param>
        /// <returns></returns>
        bool Delete(Product product);
        /// <summary>
        /// Tries to add the category by specific category and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="product">The specific product to add category.</param>
        /// <param name="category">The specific category to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        bool AddCategory(Product product, Category category);

        /// <summary>
        /// Tries to remove the category by specific category and returns whether the removal is successful or not.
        /// </summary>
        /// <param name="product">The specific product to remove category.</param>
        /// <param name="category">The specific category to be removed.</param>
        /// <returns>Boolean represents whether the removal is successful or not.</returns>
        bool RemoveCategory(Product product, Category category);
        
        /// <summary>
        /// Updates product if been modyfied.
        /// </summary>
        /// <param name="product">The specific product for updating.</param>
        /// <returns></returns>
        bool Update(Product product);

        /// <summary>
        /// Sets the description of the specific product
        /// </summary>
        /// <param name="product">The specific product to set description</param>
        /// <param name="composition">The description of the specific product</param>
        void SetComposition(Product product, string composition);
    }
}
