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
        public bool Add(Product product)
        {
            if (IsProductsInDB(p =>
                    string.Equals(p.Name, product.Name, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            product.Id = new Guid().ToString();
            _productRepository.Add(_mapper.Map<ProductDB>(product));

            return true;
        }

        /// <summary>
        /// Tries to add a new product by barcode and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="barcode">Barcode of the product to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        public bool Add(Barcode barcode)
        {
            if (IsProductsInDB(
                p => string.Equals(p.Barcode.Code, barcode.Code, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var newProduct = new Product { Id = new Guid().ToString(), Barcode = barcode, Name = barcode.ProductName };
            _productRepository.Add(_mapper.Map<ProductDB>(newProduct));

            return true;
        }

        /// <summary>
        /// Tries to add a new product by name and returns whether the addition is successful or not.
        /// </summary>
        /// <param name="name">The name of the product to be added.</param>
        /// <returns>Boolean represents whether the addition is successful or not.</returns>
        public bool Add(string name)
        {
            var product = new Product { Name = name };

            return Add(product);
        }

        /// <summary>
        /// Gets the product by its id.
        /// </summary>
        /// <param name="id">The id of the product.</param>
        /// <returns>The product with the specific id.</returns>
        public Product GetById(string id)
        {
            return _mapper.Map<Product>(_productRepository.GetById(id));
        }

        /// <summary>
        /// Gets the product by its barcode.
        /// </summary>
        /// <param name="barcode">The barcode of the product.</param>
        /// <returns>The product with the specific barcode.</returns>
        public Product Get(Barcode barcode)
        {
            return _mapper.Map<Product>(_productRepository.SelectWhere(p =>
                    string.Equals(p.Barcode.Code, barcode.Code, StringComparison.OrdinalIgnoreCase)).First());
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>All product in the application.</returns>
        public IEnumerable<Product> GetAll()
        {
            return _mapper.Map<IEnumerable<Product>>(_productRepository.SelectAll());
        }

        /// <summary>
        /// Gets product by its name.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <returns>Product with the specific name.</returns>
        public Product GetByName(string name) =>
            _mapper.Map<Product>(_productRepository.SelectWhere(p =>
            string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase)).First());

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
            return _mapper.Map<IEnumerable<Product>>(_productRepository.SelectByCategory(_mapper.Map<CategoryDB>(category)));
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
            _productRepository.Delete(productFromDB);

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
            _productRepository.Update(_mapper.Map<ProductDB>(product));

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
            _productRepository.Delete(productFromDB);

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
            _productRepository.Update(productFromDB);

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
            _productRepository.Update(productFromDB);

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
            _productRepository.Update(productFromDB);

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
            _productRepository.Update(productFromDB);

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
