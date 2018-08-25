using System;
using System.Text;
using System.Collections.Generic;
using AutoMapper;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Mappings;
using WasteProducts.Logic.Services;

namespace WasteProducts.Logic.Tests.Product_Tests
{
    /// <summary>
    /// Summary description for ProductService_Test
    /// </summary>
    [TestFixture]
    class ProductService_Test
    {
        [Test]
        public void AddByBarcode_InsertNewProduct_returns_true()
        {
            var barcode = new Barcode();

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            var mapper = new Mapper(config);
            var mockProductRepository = new Mock<IProductRepository>();
               // .Setup(repo => repo.SelectWhere(delegate(ProductDB db) {  }));
            var productService = new ProductService(mockProductRepository.Object, mapper);

            var result = productService.AddByBarcode(barcode);

            Assert.That(result, Is.EqualTo(false));
        }
    }
}
