using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Services
{
    /// <summary>
    /// This interface provides product methods
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Tries to add a new product by barcode and returns whether the addition is successful or not
        /// </summary>
        /// <param name="barcode">Barcode of the product to be added</param>
        /// <returns>Boolean represents whether the addition is successful or not</returns>
        bool AddByBarcode(Barcode barcode);

        /// <summary>
        /// Tries to add a new product by name and returns whether the addition is successful or not
        /// </summary>
        /// <param name="name">The name of the product to be added</param>
        /// <returns>Boolean represents whether the addition is successful or not</returns>
        bool AddByName(string name);

        /// <summary>
        /// Tries to delete the product by barcode and returns whether the deletion is successful or not
        /// </summary>
        /// <param name="barcode">Barcode of the product to be deleted</param>
        /// <returns>Boolean represents whether the deletion is successful or not</returns>
        bool DeleteByBarcode(Barcode barcode);

        /// <summary>
        /// Tries to delete the product by name and returns whether the deletion is successful or not
        /// </summary>
        /// <param name="name">The name of the product to be deleted</param>
        /// <returns>Boolean represents whether the deletion is successful or not</returns>
        bool DeleteByName(string name);

        /// <summary>
        /// Tries to add the category by specific category and returns whether the addition is successful or not
        /// </summary>
        /// <param name="product">The specific product to add category</param>
        /// <param name="category">The specific category to be added</param>
        /// <returns>Boolean represents whether the addition is successful or not</returns>
        bool AddCategory(Product product, Category category);

        /// <summary>
        /// Tries to remove the category by specific category and returns whether the removal is successful or not
        /// </summary>
        /// <param name="product">The specific product to remove category</param>
        /// <param name="category">The specific category to be removed</param>
        /// <returns>Boolean represents whether the removal is successful or not</returns>
        bool RemoveCategory(Product product, Category category);

        /// <summary>
        /// Sets the price of the specific product
        /// </summary>
        /// <param name="product">The specific product to set price</param>
        /// <param name="price">The price of the specific product</param>
        void SetPrice(Product product, decimal price);

        /// <summary>
        /// Allows the user to rate the specific product
        /// </summary>
        /// <param name="product">The product that the user wants to rate</param>
        /// <param name="rating">Own user rating</param>
        void Rate(Product product, int rating);

        /// <summary>
        /// Hides product for display in product lists
        /// </summary>
        /// <param name="product">The specific product to hide</param>
        void Hide(Product product);

        /// <summary>
        /// Reveal product for display in product lists
        /// </summary>
        /// <param name="product">The specific product to reveal</param>
        void Reveal(Product product);

        /// <summary>
        /// Checks whether a specific product is hidden or not
        /// </summary>
        /// <param name="product">Checked specific product</param>
        /// <returns>Boolean represents whether the product is in the hidden state</returns>
        bool IsHidden(Product product);

        /// <summary>
        /// Sets the description of the specific product
        /// </summary>
        /// <param name="product">The specific product to set description</param>
        /// <param name="description">The description of the specific product</param>
        void SetDescription(Product product, string description);
    }
}
