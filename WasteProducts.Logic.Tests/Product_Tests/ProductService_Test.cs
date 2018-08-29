using System;
using System.Text;
using System.Collections.Generic;
using AutoMapper;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Models.Barcods;
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
        private Barcode barcode;
        private BarcodeDB barcodeDB;
        private Product product;
        private ProductDB productDB;
        private List<ProductDB> selectedList;
        private MapperConfiguration mapConfig;
        private Mapper mapper;
        private Mock<IProductRepository> mockProductRepository;
        private Category category;

        [SetUp]
        public void Init()
        {
            barcode = new Barcode
            {
                Id = (new Guid()).ToString(),
                Code = "456731556",
                ProductName = "Some product"
            };
            barcodeDB = new BarcodeDB
            {
                Id = (new Guid()).ToString(),
                Code = "456731556",
                ProductName = "Some product"
            };

            selectedList = new List<ProductDB>();

            mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDB>()
                    .ForMember(m => m.Created,
                        opt => opt.MapFrom(p => p.Name != null ? DateTime.UtcNow : default(DateTime)))
                    .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?) null))
                    .ForMember(m => m.Barcode, opt => opt.Ignore())
                    .ReverseMap();
                cfg.AddProfile<CategoryProfile>();
            });

             mapper = new Mapper(mapConfig);

             mockProductRepository = new Mock<IProductRepository>();

            category = new Category
            {
                Name = "Vegetables",
                Description = "Some description",
                Products = new List<Product>()
            };

            product = new Product { Id = (new Guid()).ToString(), Name = "Some name" };
            productDB = new ProductDB {Id = (new Guid()).ToString(), Name = "Some name"};
        }

        [Test]
        public void AddByBarcode_InsertsNewProduct_Returns_True()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddByBarcode(barcode);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void AddByBarcode_DoesNotInsertNewProduct_Returns_False()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddByBarcode(barcode);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddByBarcode_InsertsNewProduct_CallsMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByBarcode(barcode);

            mockProductRepository.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void AddByBarcode_DoesNotInsertNewProduct_DoesNotCallMethod_AddOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByBarcode(barcode);

            mockProductRepository.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void AddByName_InsertsNewProduct_Returns_True()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddByName(It.IsAny<string>());

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void AddByName_DoesNotInsertNewProduct_Returns_False()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddByName(It.IsAny<string>());

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddByName_InsertsNewProduct_CallsMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByName(It.IsAny<string>());

            mockProductRepository.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void AddByName_DoesNotInsertNewProduct_DoesNotCallMethod_AddOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByName(It.IsAny<string>());

            mockProductRepository.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void AddCategoty_AddsCategoryInProductWithoutCategory_Returns_True()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddCategory(product, category);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void AddCategoty_DoesNotAddCategoryInProductWithCategory_Returns_False()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.AddCategory(product, category);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddCategoty_AddsCategoryInProductWithoutCategory_CallsMethod_UpdateOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddCategory(product, category);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void AddCategoty_DoesNotAddCategoryInProductWithCategory_DoesNotCallMethod_UpdateOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddCategory(product, category);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void DeleteByBarcode_DeletesProduct_Returns_True()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.DeleteByBarcode(barcode);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void DeleteByBarcode_DoNotDeletesProduct_Returns_False()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.DeleteByBarcode(barcode);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void DeleteByBarcode_DeletesProduct_CallsMethod_DeleteOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.DeleteByBarcode(barcode);

            mockProductRepository.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void DeleteByBarcode_DoesNotDeleteProduct_DoesNotCallMethod_DeleteOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.DeleteByBarcode(barcode);

            mockProductRepository.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void DeleteByName_DeletesProduct_Returns_True()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.DeleteByName(It.IsAny<string>());

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void DeleteByName_DoesNotDeleteProduct_Returns_False()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.DeleteByName(It.IsAny<string>());

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void DeleteByName_DeletesProduct_CallsMethod_DeleteOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.DeleteByName(It.IsAny<string>());

            mockProductRepository.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void DeleteByName_DoNotDeleteProduct_DoesNotCallMethod_DeleteOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.DeleteByName(It.IsAny<string>());

            mockProductRepository.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void Hide_HidesProduct_CallsMethod_HideOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.Hide(product);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void Hide_PassesNull_DoesNotCallMethod_HideOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.Hide(null);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void Hide_PassesProductWichDoesNotExistInDB_DoesNotCallMethod_HideOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.Hide(product);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }
    }
}
