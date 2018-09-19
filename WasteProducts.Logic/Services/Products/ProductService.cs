using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public Task<string> Add(Barcode barcode)
        {
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
            return _productRepository.AddAsync(_mapper.Map<ProductDB>(newProduct)).ContinueWith(i=>i.;
        }

            return _productRepository.AddAsync(_mapper.Map<ProductDB>(newProduct))
                .ContinueWith(t => t.Result);
        }

        public Task<Product> GetById(string id)
        {
            return _productRepository.GetByIdAsync(id)
                .ContinueWith(t => _mapper.Map<Product>(t.Result));
        }

        public Task<Product> GetByBarcode(Barcode barcode)
        {
            return _productRepository.SelectWhereAsync(p =>
                    string.Equals(p.Barcode.Code, barcode.Code, StringComparison.OrdinalIgnoreCase))
                 .ContinueWith(t => _mapper.Map<Product>(t.Result.First()));
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            return _productRepository.SelectAllAsync()
                .ContinueWith(t => _mapper.Map<IEnumerable<Product>>(t.Result));
        }

        public Task<Product> GetByName(string name)
        {
            return _productRepository.SelectWhereAsync(p =>
                 string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase))
                 .ContinueWith(t => _mapper.Map<Product>(t.Result.First()));
        }
           
        public Task<IEnumerable<Product>> GetByCategory(Category category)
        {
            return _productRepository.SelectByCategoryAsync(_mapper.Map<CategoryDB>(category))
                .ContinueWith(t => _mapper.Map<IEnumerable<Product>>(t.Result));
        }
                
        public Task Update(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.CurrentCultureIgnoreCase),
                out IEnumerable<ProductDB> products)) return null;

            return _productRepository.UpdateAsync(_mapper.Map<ProductDB>(product));
        }

        public Task Delete(string id)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, id, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return null;

            return _productRepository.DeleteAsync(_mapper.Map<ProductDB>(products.First()));
        }

        public Task AddToCategory(string productId, string categoryId)
        {
            if (!IsProductsInDB(p =>
                string.Equals(p.Id, productId, StringComparison.Ordinal),
                out var products)) return null;

            var productFromDB = products.ToList().First();

            //productFromDB.Category = ??

            return _productRepository.UpdateAsync(productFromDB);
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
