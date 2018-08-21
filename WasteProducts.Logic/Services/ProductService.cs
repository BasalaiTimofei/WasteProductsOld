using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    class ProductService : IProductService
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

            var newProduct = new Product {Barcode = barcode, Name = barcode.ProductName};
            _productRepository.Add(_mapper.Map<ProductDB>(newProduct));

            return true;
        }
        
        public bool AddByName(string name)
        {
            if (IsProductsInDB(p =>
                string.Equals(p.Name, name, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var newProduct = new Product {Name = name};
            _productRepository.Add(_mapper.Map<ProductDB>(newProduct));

            return true;
        }
        
        public bool AddCategory(Product product, Category category)
        {
            if (!IsProductsInDB(p =>
                string.Equals(p.Name, product.Name, StringComparison.CurrentCultureIgnoreCase),
                out var products)) return false;

            var productFormDB = products.ToList().First();
            productFormDB.Category = Mapper.Map<CategoryDB>(category);
            _productRepository.Update(productFormDB);

            return true;
        }

        public bool DeleteByBarcode(Barcode barcode)
        {
            
        }

        public bool DeleteByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Hide(Product product)
        {
            throw new NotImplementedException();
        }

        public bool IsHidden(Product product)
        {
            throw new NotImplementedException();
        }

        public void Rate(Product product, int rating)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCategory(Product product, Category category)
        {
            throw new NotImplementedException();
        }

        public void Reveal(Product product)
        {
            throw new NotImplementedException();
        }

        public void SetDescription(Product product, string description)
        {
            throw new NotImplementedException();
        }

        public void SetPrice(Product product, decimal price)
        {
            throw new NotImplementedException();
        }

        private bool IsProductsInDB(Predicate<ProductDB> conditionPredicate, out IEnumerable<ProductDB> products)
        {
            products = _productRepository.SelectWhere(conditionPredicate);
            return products.Any();
        }
    }
}
