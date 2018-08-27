using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public bool AddByBarcode(Barcode barcode)
        {
            if (IsProductsInDB(
                p => string.Equals(p.Barcode.Code, barcode.Code, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var newProduct = new Product {Id = new Guid().ToString(), Barcode = barcode, Name = barcode.ProductName};
            _productRepository.Add(_mapper.Map<ProductDB>(newProduct));

            return true;
        }
        
        public bool AddByName(string name)
        {
            if (IsProductsInDB(p =>
                string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var newProduct = new Product { Id = new Guid().ToString(), Name = name};
            _productRepository.Add(_mapper.Map<ProductDB>(newProduct));

            return true;
        }
        
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

        public bool DeleteByBarcode(Barcode barcode)
        {
            if (!IsProductsInDB(
                p => string.Equals(p.Barcode.Code, barcode.Code, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            _productRepository.Delete(productFromDB);

            return true;
        }

        public bool DeleteByName(string name)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            _productRepository.Delete(productFromDB);

            return true;
        }

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

        public bool IsHidden(Product product)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return false;

            var productFromDB = products.ToList().First();

            return productFromDB.IsHidden;
        }

        public void Rate(Product product, int rating)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return;

            var productFromDB = products.ToList().First();
            productFromDB.AvgRating = (productFromDB.AvgRating * productFromDB.RateCount + rating) / ++productFromDB.RateCount;
            _productRepository.Update(productFromDB);
        }

        public bool RemoveCategory(Product product, Category category)
        {
            if (!IsProductsInDB(p =>
                string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return false;

            var productFromDB = products.ToList().First();
            productFromDB.Category = null;
            _productRepository.Update(productFromDB);

            return true;
        }

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

        public void SetDescription(Product product, string description)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return;

            var productFromDB = products.ToList().First();
            productFromDB.Description = description;
            _productRepository.Update(productFromDB);
        }

        public void SetPrice(Product product, decimal price)
        {
            if (!IsProductsInDB(p =>
                    string.Equals(p.Id, product.Id, StringComparison.Ordinal),
                out var products)) return;

            var productFromDB = products.ToList().First();
            productFromDB.Price = price;
            _productRepository.Update(productFromDB);
        }

        private bool IsProductsInDB(Predicate<ProductDB> conditionPredicate, out IEnumerable<ProductDB> products)
        {
            products = _productRepository.SelectWhere(conditionPredicate);
            return products.Any();
        }
    }
}
