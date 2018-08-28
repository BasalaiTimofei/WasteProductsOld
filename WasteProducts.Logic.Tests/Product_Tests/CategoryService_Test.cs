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
    /// Summary description for Category_Service_Test
    /// </summary>
    [TestFixture]
    class CategoryService_Test
    {
        private Mock<ICategoryRepository> mockCategoryRepo;
        private List<CategoryDB> selectedList;
        private MapperConfiguration mapConfig;
        private Mapper mapper;
        private Category category;
        private CategoryDB categoryDB;
        private List<string> names;

        [SetUp]
        public void Init()
        {
            mockCategoryRepo = new Mock<ICategoryRepository>();
            selectedList = new List<CategoryDB>();
            names = new List<string>() { "Milk products", "Meat" };

            mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CategoryProfile>();
            });

            mapper = new Mapper(mapConfig);

            category = new Category { Name = "Meat" };
            categoryDB = new CategoryDB { Name = "Milk products" };
        }

        [Test]
        public void AddByName_InsertNewCategory_ReturnsTrue()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.AddByName(It.IsAny<string>());

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void AddByName_InsertedCategoryExists_ReturnsFalse()
        {
            selectedList.Add(categoryDB);

            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.AddByName(It.IsAny<string>());

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddByName_InsertNewCategory_AddMethodOfRepoIsCalledOnce()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.AddByName(It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.Add(It.IsAny<CategoryDB>()), Times.Once);
        }

        [Test]
        public void AddByName_InsertedCategoryExists_AddMethodOfRepoIsNeverCalled()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.AddByName(It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.Add(It.IsAny<CategoryDB>()), Times.Once);
        }

        [Test]
        public void AddRange_InsertTwoNewCategories_ReturnsTrue()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.AddRange(names);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void AddRange_InsertTwoNewCategories_AddMethodOfRepoIsCalledTwice()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.AddRange(names);

            mockCategoryRepo.Verify(m => m.Add(It.IsAny<CategoryDB>()), Times.Exactly(2));
        }

        [Test]
        public void AddRange_InsertTwoExistingCategories_ReturnFalseAndAddMethodOfRepoNeverCalled()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.AddRange(names);

            Assert.That(result, Is.EqualTo(false));
            mockCategoryRepo.Verify(m => m.Add(It.IsAny<CategoryDB>()), Times.Never);
        }

        [Test]
        public void Get_CategoryNotFound_ReturnsNull()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Get(It.IsAny<string>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_CategoryWasFound_NamesAreTheSame()
        {
            selectedList.Add(new CategoryDB { Name = "Meat" });
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Get(It.IsAny<string>());

            Assert.That(result, Is.TypeOf(typeof(Category)));
            Assert.AreEqual("Meat", result.Name);
        }

        [Test]
        public void SetDescription_CategoryNotFound_UpdateMethodIsNeverCalled()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.SetDescription(category, It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.Update(It.IsAny<CategoryDB>()), Times.Never);
        }

        [Test]
        public void SetDescription_CategoryWasFound_DescriptionIsAddedAndUpdateMethodIsCalledOnce()
        {
            selectedList.Add(categoryDB);
            var description = "Some description";
            mockCategoryRepo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.SetDescription(category, description);

            Assert.AreEqual(selectedList[0].Description, description);
            mockCategoryRepo.Verify(m => m.Update(It.IsAny<CategoryDB>()), Times.Once);
        }
    }
}
