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

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public bool AddByBarcode(Barcode barcode)
        {
            throw new NotImplementedException();
        }

        //походу полезно будет довабить в репозиторий поиск по имени
        public bool AddByName(string name)
        {
            var product = _productRepository.SelectAll()
                .First(p => string.Equals(p.Name.ToLower(), name.ToLower(), StringComparison.CurrentCultureIgnoreCase));

            if (product != null) return false;

            var newProduct = new Product {Name = name};
            _productRepository.Add(Mapper.Map<ProductDB>(newProduct));

            return true;
        }

        public bool AddCategory(Product product, Category category)
        {
            throw new NotImplementedException();
        }

        public bool DeleteByBarcode(Barcode barcode)
        {
            throw new NotImplementedException();
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
    }
}
