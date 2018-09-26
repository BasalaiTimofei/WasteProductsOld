using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ninject;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Common.Services.Products;

namespace WasteProducts.Logic.Services.Products
{
    /// <summary>
    /// Implementation of IProductService.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBarcodeService _barcodeService;
        private readonly IMapper _mapper;
        private bool _disposed;

        public ProductService(IProductRepository productRepository, 
            ICategoryRepository categoryRepository, 
            IBarcodeService barcodeService, 
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public Task<string> Add(Stream imageStream)
        {
            if (imageStream == null) return null;

            var barcode = _barcodeService.GetBarcodeAsync(imageStream).Result;
            if (barcode == null) return null;

            if (IsProductsInDB(
                p => string.Equals(p.Barcode.Code, barcode.Code, StringComparison.CurrentCultureIgnoreCase),
                out var products))
            {
                return null;
            }

            var newProduct = new Product
            {
                Barcode = barcode,
                Name = barcode.ProductName
            };
            
            return _productRepository.AddAsync(_mapper.Map<ProductDB>(newProduct))
                .ContinueWith(t => t.Result);
        }

        /// <inheritdoc/>
        public Task<Product> GetById(string id)
        {
            return _productRepository.GetByIdAsync(id)
                .ContinueWith(t => _mapper.Map<Product>(t.Result));
        }

        /// <inheritdoc/>
        public Task<Product> GetByBarcode(Barcode barcode)
        {
            return _productRepository.SelectWhereAsync(p =>
                    string.Equals(p.Barcode.Code, barcode.Code, StringComparison.OrdinalIgnoreCase))
                 .ContinueWith(t => _mapper.Map<Product>(t.Result.First()));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Product>> GetAll()
        {
            return _productRepository.SelectAllAsync()
                .ContinueWith(t => _mapper.Map<IEnumerable<Product>>(t.Result));
        }

        /// <inheritdoc/>
        public Task<Product> GetByName(string name)
        {
            return _productRepository.SelectWhereAsync(p =>
                 string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase))
                 .ContinueWith(t => _mapper.Map<Product>(t.Result.First()));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Product>> GetByCategory(Category category)
        {
            return _productRepository.SelectByCategoryAsync(_mapper.Map<CategoryDB>(category))
                .ContinueWith(t => _mapper.Map<IEnumerable<Product>>(t.Result));
        }

        /// <inheritdoc/>
        public Task Update(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.CurrentCultureIgnoreCase),
                out IEnumerable<ProductDB> products)) return null;

            return _productRepository.UpdateAsync(_mapper.Map<ProductDB>(product));
        }

        /// <inheritdoc/>
        public Task Delete(string id)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, id, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return null;

            return _productRepository.DeleteAsync(_mapper.Map<ProductDB>(products.First()));
        }

        /// <inheritdoc/>
        public Task AddToCategory(string productId, string categoryId)
        {
            if (!IsProductsInDB(p =>
                string.Equals(p.Id, productId, StringComparison.Ordinal),
                out var products)) return null;

            var productFromDB = products.ToList().First();

            productFromDB.Category = _categoryRepository.GetByIdAsync(categoryId).Result;

            return _productRepository.UpdateAsync(productFromDB);
        }

        /// <inheritdoc/>
        private bool IsProductsInDB(Predicate<ProductDB> conditionPredicate, out IEnumerable<ProductDB> products)
        {
            products = _productRepository.SelectWhereAsync(conditionPredicate).Result;
            return products.Any();
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

        ~ProductService()
        {
            Dispose();
        }
    }
}
