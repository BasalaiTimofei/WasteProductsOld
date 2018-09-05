using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ninject;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    /// <summary>
    /// Implementation of IProductService
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private bool _disposed;

        public ProductService(IProductRepository productRepository, [Named("ProductService")] IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Tries to add a new product and returns whether the addition is successful or not
        /// </summary>
        /// <param name="product">The product to be added</param>
        /// <returns>Boolean represents whether the addition is successful or not</returns>
        public bool Add(Product product)
        {
            if (product == null || IsProductsInDB(p =>
                    string.Equals(p.Name, product.Name, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            product.Id = new Guid().ToString();
            _productRepository.Add(_mapper.Map<ProductDB>(product));

            return true;
        }

        /// <summary>
        /// Tries to add a new product by barcode and returns whether the addition is successful or not
        /// </summary>
        /// <param name="barcode">Barcode of the product to be added</param>
        /// <returns>Boolean represents whether the addition is successful or not</returns>
        public bool AddByBarcode(Barcode barcode)
        {
            if (IsProductsInDB(
                p => string.Equals(p.Barcode.Code, barcode.Code, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var newProduct = new Product {Id = new Guid().ToString(), Barcode = barcode, Name = barcode.ProductName};
            _productRepository.Add(_mapper.Map<ProductDB>(newProduct));

            return true;
        }

        /// <summary>
        /// Tries to add a new product by name and returns whether the addition is successful or not
        /// </summary>
        /// <param name="name">The name of the product to be added</param>
        /// <returns>Boolean represents whether the addition is successful or not</returns>
        public bool AddByName(string name)
        {
            if (name == null) return false;
            var product = new Product { Name = name };

            return Add(product);
        }

        /// <summary>
        /// Gets the product by its id
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns>The product with the specific id</returns>
        public Product GetById(string id)
        {
            return id == null ? null : _mapper.Map<Product>(_productRepository.GetById(id));
        }

        /// <summary>
        /// Gets the product by its barcode
        /// </summary>
        /// <param name="barcode">The barcode of the product</param>
        /// <returns>The product with the specific barcode</returns>
        public Product GetByBarcode(Barcode barcode)
        {
            return barcode == null
                ? null
                : _mapper.Map<Product>(_productRepository.SelectWhere(p =>
                    string.Equals(p.Barcode.Code, barcode.Code, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Gets product by its name.
        /// </summary>
        /// <param name="name">Name of the product.</param>
        /// <returns>Product with the specific name.</returns>
        public async Task<Product> GetByNameAsync(string name)
        {
            return name == null ? null : _mapper.Map<Product>(await _productRepository.GetByNameAsync(name));
        }

        /// <summary>
        /// Tries to delete the product by barcode and returns whether the deletion is successful or not
        /// </summary>
        /// <param name="barcode">Barcode of the product to be deleted</param>
        /// <returns>Boolean represents whether the deletion is successful or not</returns>
        public bool DeleteByBarcode(Barcode barcode)
        {
            if (!IsProductsInDB(
                p => string.Equals(p.Barcode.Code, barcode.Code, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            _productRepository.Delete(productFromDB);

            return true;
        }

        /// <summary>
        /// Tries to delete the product by name and returns whether the deletion is successful or not
        /// </summary>
        /// <param name="name">The name of the product to be deleted</param>
        /// <returns>Boolean represents whether the deletion is successful or not</returns>
        public bool DeleteByName(string name)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            _productRepository.Delete(productFromDB);

            return true;
        }

        /// <summary>
        /// Tries to add the category by specific category and returns whether the addition is successful or not
        /// </summary>
        /// <param name="product">The specific product to add category</param>
        /// <param name="category">The specific category to be added</param>
        /// <returns>Boolean represents whether the addition is successful or not</returns>
        public bool AddCategory(Product product, Category category)
        {
            if (!IsProductsInDB(p =>
                string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            productFromDB.Category = _mapper.Map<CategoryDB>(category);
            _productRepository.Update(productFromDB);

            return true;
        }

        /// <summary>
        /// Tries to remove the category by specific category and returns whether the removal is successful or not
        /// </summary>
        /// <param name="product">The specific product to remove category</param>
        /// <param name="category">The specific category to be removed</param>
        /// <returns>Boolean represents whether the removal is successful or not</returns>
        public bool RemoveCategory(Product product, Category category)
        {
            if (!IsProductsInDB(p =>
                string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            if (productFromDB.Category == null) return true;

            productFromDB.Category = null;
            _productRepository.Update(productFromDB);

            return true;
        }

        /// <summary>
        /// Sets the price of the specific product
        /// </summary>
        /// <param name="product">The specific product to set price</param>
        /// <param name="price">The price of the specific product</param>
        public void SetPrice(Product product, decimal price)
        {
            if (price <= 0M || !IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return;

            var productFromDB = products.ToList().First();
            productFromDB.Price = price;
            _productRepository.Update(productFromDB);
        }

        /// <summary>
        /// Allows the user to rate the specific product
        /// </summary>
        /// <param name="product">The product that the user wants to rate</param>
        /// <param name="rating">Own user rating</param>
        public void Rate(Product product, int rating)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return;

            var productFromDB = products.ToList().First();
            if (productFromDB.AvgRating == null) productFromDB.AvgRating = 0d;
            productFromDB.AvgRating = (productFromDB.AvgRating * productFromDB.RateCount + rating) / ++productFromDB.RateCount;
            _productRepository.Update(productFromDB);
        }

        /// <summary>
        /// Hides product for display in product lists
        /// </summary>
        /// <param name="product">The specific product to hide</param>
        public void Hide(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return;

            var productFromDB = products.ToList().First();
            if (productFromDB.IsHidden) return;

            productFromDB.IsHidden = product.IsHidden = true;
            _productRepository.Update(productFromDB);
        }

        /// <summary>
        /// Reveal product for display in product lists
        /// </summary>
        /// <param name="product">The specific product to reveal</param>
        public void Reveal(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return;

            var productFromDB = products.ToList().First();
            if (!productFromDB.IsHidden) return;

            productFromDB.IsHidden = false;
            _productRepository.Update(productFromDB);
        }

        /// <summary>
        /// Checks whether a specific product is hidden or not
        /// </summary>
        /// <param name="product">Checked specific product</param>
        /// <returns>Boolean represents whether the product is in the hidden state</returns>
        public bool IsHidden(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return false;

            var productFromDB = products.ToList().First();

            return productFromDB.IsHidden;
        }

        /// <summary>
        /// Sets the description of the specific product
        /// </summary>
        /// <param name="product">The specific product to set description</param>
        /// <param name="description">The description of the specific product</param>
        public void SetDescription(Product product, string description)
        {
            if (description == null || !IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return;

            var productFromDB = products.ToList().First();
            productFromDB.Description = description;
            _productRepository.Update(productFromDB);
        }
     
        private bool IsProductsInDB(Predicate<ProductDB> conditionPredicate, out IEnumerable<ProductDB> products)
        {
            products = _productRepository.SelectWhere(conditionPredicate);
            return products.Any();
        }

        ~ProductService()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _productRepository?.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}
