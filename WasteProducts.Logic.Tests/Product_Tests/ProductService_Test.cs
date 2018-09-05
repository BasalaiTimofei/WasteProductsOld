using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Mappings;
using WasteProducts.Logic.Services;

#region FluentAssertions descreiption
//https://fluentassertions.com/documentation/

//As you may have noticed, the purpose of this open-source project is to not only be the best
//assertion framework in the.NET realm, but to also demonstrate high-quality code. We heavily
//practice Test Driven Development and one of the promises TDD makes is that unit tests can be
//treated as your API’s documentation. So although you are free to go through the many examples
//here, please consider to analyze the many unit tests. (developer's comment)
#endregion

namespace WasteProducts.Logic.Tests.Product_Tests
{
    /// <summary>
    /// Summary description for ProductService_Test
    /// </summary>
    [TestFixture]
    class ProductService_Test
    {
        private const string productName = "Some name";

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
        //TODO Проверить чисто на null
        public void AddByBarcore_BarcodeIsNotNull()
        {
            var productService = new ProductService(mockProductRepository.Object, mapper);

            var isSuccess = productService.AddByBarcode(barcode);

            isSuccess.Should().BeTrue().And.Should().NotBe(null);
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
            var result = productService.AddByName(productName);

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
        public void AddName_NameIsNull()
        {
            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByName(null);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            mockProductRepository.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void AddByName_InsertsNewProduct_CallsMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.AddByName(productName);

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
        public void DeletetByName_IfNameIsEmpty_CalledNever()
        {
            var prod = new ProductDB { Name = "" };
            selectedList.Add(prod);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.DeleteByName(prod.Name);

            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            prod.Name.Should().BeEmpty();

            mockProductRepository.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void DeleteByName_IfNameIsNull_CalledNever()
        {
            var prod = new ProductDB { Name = null };
            selectedList.Add(prod);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.DeleteByName(prod.Name);

            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            prod.Name.Should().BeNull();
            mockProductRepository.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Never);
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

        [Test]
        public void IsHidden_PassesHiddenProduct_Returns_True()
        {
            productDB.IsHidden = true;
            product.IsHidden = true;
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.IsHidden(product);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void IsHidden_PassesNotHiddenProduct_Returns_False()
        {
            productDB.IsHidden = false;
            product.IsHidden = false;
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.IsHidden(product);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void IsHidden_PassesNull_Returns_False()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.IsHidden(null);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void IsHidden_PassesProductWichDoesNotExistInDB_Returns_False()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            var result = productService.IsHidden(product);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void Rate_SetsRating_CallsMethod_UpdateOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.Rate(product, It.IsAny<int>());

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void Rate_PassedProductIsNull_DoesNotCallMethod_UpdateOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.Rate(null, It.IsAny<int>());

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void Rate_CheckingResultAfterRating()
        {
            var prodDb = new ProductDB
            {
                AvgRating = 5d,
                RateCount = 4,
                Id = Guid.NewGuid().ToString()
            };

            selectedList.Add(prodDb);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);

            productService.Rate(product, 4);

            Assert.AreEqual(4.8d, selectedList[0].AvgRating);
            Assert.AreEqual(5, selectedList[0].RateCount);
        }

        [Test]
        public void RemoveCategory_Removed_ReturnsTrue()
        {
            var categoryDb = new CategoryDB();
            var category = new Category();
            var catList = new List<CategoryDB>();

            selectedList.Add(new ProductDB());
            catList.Add(categoryDb);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.RemoveCategory(product, category).Should().BeTrue();
        }

        [Test]
        public void RemoveCategory_NotRemoved_ReturnsFalse()
        {
            var category = new Category();
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.RemoveCategory(product, category).Should().BeFalse();
        }

        [Test]
        //Переиновать
        public void RemoveCategory_RemovesCategoryInProductWithoutCategory_Method_UpdateOfRepositoryIsNeverCalled()
        {
            var categoryDb = new CategoryDB();
            var catList = new List<CategoryDB> { categoryDb };

            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.RemoveCategory(product, category);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void Reveal_OpensAProductToDisplaying_CallsOnce()
        {
            productDB.Equals(product);
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            productDB.IsHidden = true;

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.Reveal(product);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void Reveal_IfProductDoesntHidden_CallsNever()
        {
            productDB.Equals(product);
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            productDB.IsHidden = false;

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.Reveal(product);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void SetDescription_Success()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.SetDescription(product, "Оч крутой кисель");

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void SetDescription_NotSuccess()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.SetDescription(product, "Оч крутой кисель");

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void SetDescription_IfDescriptionIsNull()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.SetDescription(product, null);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void SetPrice_SuccessfullySetPrice()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.SetPrice(product, 10.22m);
            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void SetPrice_PriceIsZero()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.SetPrice(product, 0);

            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void SetPrice_DoNotSetsThePrice_DoesNotcallMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(selectedList);

            var productService = new ProductService(mockProductRepository.Object, mapper);
            productService.SetPrice(product, 15.1m);
            mockProductRepository.Verify(m => m.Update(It.IsAny<ProductDB>()), Times.Never);
        }
    }
}
