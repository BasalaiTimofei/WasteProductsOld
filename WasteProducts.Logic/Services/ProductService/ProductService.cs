using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _prodRepo;

        public ProductService(IProductRepository repo)
        {
            _prodRepo = repo;
        }

        public bool AddByBarcode(Barcode barcode)
        {
            throw new NotImplementedException();
        }

        public bool AddByName(string name)
        {
            throw new NotImplementedException();
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
