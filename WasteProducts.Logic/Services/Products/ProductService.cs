using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            _barcodeService = barcodeService;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<string> AddAsync(Stream imageStream)
        {
            var code = _barcodeService.ParseBarcodePhoto(imageStream);
            if (string.IsNullOrEmpty(code))
            {
                throw new ValidationException("Barcode wasn't read from the uploaded photo");
            }

            var barcodeDB = await _barcodeService.GetBarcodeFromDBAsync(code);
            if (barcodeDB != null && IsProductsInDB(p => p.Barcode != null &&
                    string.Equals(p.Barcode.Code, barcodeDB.Code, StringComparison.CurrentCultureIgnoreCase), out var products))
            {
                return products.First().Id;
            }
            else
            {
                var barcode = await _barcodeService.GetBarcodeFromCatalogAsync(code);
                var newProduct = new Product
                {
                    Barcode = barcode,
                    Name = barcode.ProductName,
                    Composition = barcode.Composition,
                    Brand = barcode.Brand,
                    Country = barcode.Country,
                    Weight = barcode.Weight,
                    PicturePath = barcode.PicturePath,
                };

                return await _productRepository.AddAsync(_mapper.Map<ProductDB>(newProduct));
            }
        }

        /// <inheritdoc/>
        public Task<string> AddAsync(string name)
        {
            if (IsProductsInDB(
                p => string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var products))
            {
                return null;
            }

            var newProduct = new Product
            {
                Name = name,
                Barcode = new Barcode()
            };

            return _productRepository.AddAsync(_mapper.Map<ProductDB>(newProduct))
                .ContinueWith(t => t.Result);
        }

        /// <inheritdoc/>
        public Task<Product> GetByIdAsync(string id)
        {
            return _productRepository.GetByIdAsync(id)
                .ContinueWith(t => _mapper.Map<Product>(t.Result));
        }

        /// <inheritdoc/>
        public Task<Product> GetByBarcodeAsync(Barcode barcode)
        {
            return _productRepository.SelectWhereAsync(p =>
                    string.Equals(p.Barcode.Code, barcode.Code, StringComparison.OrdinalIgnoreCase))
                 .ContinueWith(t => _mapper.Map<Product>(t.Result.First()));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var subResult = await _productRepository.SelectAllAsync();

            IEnumerable<Product> result = null;
            try
            {
                result = _mapper.Map<IEnumerable<Product>>(subResult);
            }
            catch(Exception e)
            {

            }
            return result;

            //return _productRepository.SelectAllAsync()
            //    .ContinueWith(t => _mapper.Map<IEnumerable<Product>>(t.Result));
        }

        /// <inheritdoc/>
        public Task<Product> GetByNameAsync(string name)
        {
            return _productRepository.SelectWhereAsync(p =>
                 string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase))
                 .ContinueWith(t => _mapper.Map<Product>(t.Result.First()));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Product>> GetByCategoryAsync(Category category)
        {
            return _productRepository.SelectByCategoryAsync(_mapper.Map<CategoryDB>(category))
                .ContinueWith(t => _mapper.Map<IEnumerable<Product>>(t.Result));
        }

        /// <inheritdoc/>
        public Task UpdateAsync(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.CurrentCultureIgnoreCase),
                out IEnumerable<ProductDB> products)) return null;

            return _productRepository.UpdateAsync(_mapper.Map<ProductDB>(product));
        }

        /// <inheritdoc/>
        public Task DeleteAsync(string id)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, id, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return null;

            return _productRepository.DeleteAsync(id);
        }

        /// <inheritdoc/>
        public Task AddToCategoryAsync(string productId, string categoryId)
        {
            return _productRepository.AddToCategoryAsync(productId, categoryId);
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

        private bool IsProductsInDB(Predicate<ProductDB> conditionPredicate, out IEnumerable<ProductDB> products)
        {
            products = _productRepository.SelectWhereAsync(conditionPredicate).Result;
            return products.Any();
        }

        ~ProductService()
        {
            Dispose();
        }
    }
}
