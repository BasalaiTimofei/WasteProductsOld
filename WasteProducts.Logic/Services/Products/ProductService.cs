using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ninject;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services.Products;

namespace WasteProducts.Logic.Services.Products
{
    /// <summary>
    /// Implementation of IProductService.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private bool _disposed;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Tries to add a new product and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="product">The product to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        public bool Add(Product product, out Product addedProduct)
        {
            if (IsProductsInDB(p =>
                    string.Equals(p.Name, product.Name, StringComparison.CurrentCultureIgnoreCase),
                out var products))
            {
                addedProduct = null;
                return false;
            }

            product.Id = Guid.NewGuid().ToString();
            // todo catch exception
            _productRepository.AddAsync(_mapper.Map<ProductDB>(product));

            addedProduct = product;
            return true;
        }

        /// <summary>
        /// Tries to add a new product by barcode and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="barcode">Barcode of the product to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        public bool Add(Barcode barcode, out Product addedProduct)
        {
            if (IsProductsInDB(
                p => string.Equals(p.Barcode.Code, barcode.Code, StringComparison.CurrentCultureIgnoreCase),
                out var products))
            {
                addedProduct = null;
                return false;
            }

            var newProduct = new Product
            {
                Id = Guid.NewGuid()
                .ToString(),
                Barcode = barcode,
                Name = barcode.ProductName
            };
            _productRepository.AddAsync(_mapper.Map<ProductDB>(newProduct));

            addedProduct = newProduct;
            return true;
        }

        /// <summary>
        /// Tries to add a new product by name and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="name">The name of the product to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        public bool Add(string name, out Product addedProduct)
        {
            var product = new Product { Name = name };

            return Add(product, out addedProduct);
        }

        /// <summary>
        /// Gets the product by its id.
        /// </summary>
        /// <param name="id">The id of the product.</param>
        /// <returns>The product with the specific id.</returns>
        public Product GetById(string id)
        {
            return _mapper.Map<Product>(_productRepository.GetByIdAsync(id).Result);
        }

        /// <summary>
        /// Gets the product by its barcode.
        /// </summary>
        /// <param name="barcode">The barcode of the product.</param>
        /// <returns>The product with the specific barcode.</returns>
        public Product GetByBarcode(Barcode barcode)
        {
            return _mapper.Map<Product>(_productRepository.SelectWhereAsync(p =>
                    string.Equals(p.Barcode.Code, barcode.Code, StringComparison.OrdinalIgnoreCase)).Result.First());
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>All product in the application.</returns>
        public IEnumerable<Product> GetAll()
        {
            return _mapper.Map<IEnumerable<Product>>(_productRepository.SelectAllAsync().Result);
        }

        /// <summary>
        /// Gets product by its name.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <returns>Product with the specific name.</returns>
        public Product GetByName(string name) =>
            _mapper.Map<Product>(_productRepository.SelectWhereAsync(p =>
            string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase)).Result.First());

        /// <summary>
        /// Gets asynchronously product by its name.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <returns>Product with the specific name.</returns>
        public async Task<Product> GetByNameAsync(string name) =>
            _mapper.Map<Product>(await _productRepository.GetByNameAsync(name));

        /// <summary>
        /// Gets products by a category.
        /// </summary>
        /// <param name="category">Category whose products are to be listed</param>
        /// <returns>Product collection of the specific category.</returns>
        public IEnumerable<Product> GetByCategory(Category category)
        {
            return _mapper.Map<IEnumerable<Product>>(_productRepository.SelectByCategoryAsync(_mapper.Map<CategoryDB>(category)));
        }

        /// <summary>
        /// Tries to delete the product by barcode and returns whether the deletion is successful or not.
        /// </summary>
        /// <param name="barcode">Barcode of the product to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        public bool Delete(Barcode barcode)
        {
            if (!IsProductsInDB(
                p => string.Equals(p.Barcode.Code, barcode.Code, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            _productRepository.DeleteAsync(productFromDB);

            return true;
        }

        /// <summary>
        /// Updates product if been modyfied.
        /// </summary>
        /// <param name="product">The specific product for updating.</param>
        /// <returns></returns>
        public bool Update(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.CurrentCultureIgnoreCase),
                out IEnumerable<ProductDB> products)) return false;
            _productRepository.UpdateAsync(_mapper.Map<ProductDB>(product));

            return true;
        }

        /// <summary>
        /// Tries to delete the product by name and returns whether the deletion is successful or not.
        /// </summary>
        /// <param name="name">The name of the product to be deleted.</param>
        /// <returns>Boolean represents whether the deletion is successful or not.</returns>
        public bool Delete(string name)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            _productRepository.DeleteAsync(productFromDB);

            return true;
        }

        /// <inheritdoc />
        public bool Delete(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;
            _productRepository.DeleteAsync(_mapper.Map<ProductDB>(product));

            return true;
        }

        /// <summary>
        /// Tries to add the category and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="product">The specific product to add category.</param>
        /// <param name="category">The specific category to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        public bool AddCategory(Product product, Category category)
        {
            if (!IsProductsInDB(p =>
                string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            productFromDB.Category = _mapper.Map<CategoryDB>(category);
            _productRepository.UpdateAsync(productFromDB);

            return true;
        }

        /// <summary>
        /// Tries to remove the category by specific category and returns whether the removal is successful or not.
        /// </summary>
        /// <param name="product">The specific product to remove category.</param>
        /// <param name="category">The specific category to be removed</param>
        /// <returns>Boolean represents whether the removal is successful or not.</returns>
        public bool RemoveCategory(Product product, Category category)
        {
            if (!IsProductsInDB(p =>
                string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            if (productFromDB.Category == null) return true;

            productFromDB.Category = null;
            _productRepository.UpdateAsync(productFromDB);

            return true;
        }

        /// <summary>
        /// Hides product for display in product lists.
        /// </summary>
        /// <param name="product">The specific product to hide.</param>
        /// <returns>Boolean represents whether the hiding is successful or not.</returns>
        public bool Hide(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            if (productFromDB.IsHidden) return true;

            productFromDB.IsHidden = product.IsHidden = true;
            _productRepository.UpdateAsync(productFromDB);

            return true;
        }

        /// <summary>
        /// Reveal product for display in product lists.
        /// </summary>
        /// <param name="product">The specific product to reveal.</param>
        /// <returns>Boolean represents whether the revealing is successful or not.</returns>
        public bool Reveal(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            if (!productFromDB.IsHidden) return true;

            productFromDB.IsHidden = false;
            _productRepository.UpdateAsync(productFromDB);

            return true;
        }

        /// <summary>
        /// Checks if the specific product is hidden.
        /// </summary>
        /// <param name="product">The specific product under checking.</param>
        /// <returns>Boolean represents whether the product is in the hidden state.</returns>
        public bool? IsHidden(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return null;

            var productFromDB = products.ToList().First();

            return productFromDB.IsHidden;
        }

        /// <summary>
        /// Sets the description of the specific product.
        /// </summary>
        /// <param name="product">The specific product to set description.</param>
        /// <param name="composition">The description of the specific product.</param>
        public void SetComposition(Product product, string composition)
        {
            if (composition == null || !IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return;

            var productFromDB = products.ToList().First();
            productFromDB.Composition = composition;
            _productRepository.UpdateAsync(productFromDB);
        }

        private bool IsProductsInDB(Predicate<ProductDB> conditionPredicate, out IEnumerable<ProductDB> products)
        {
            products = _productRepository.SelectWhereAsync(conditionPredicate).Result;
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
