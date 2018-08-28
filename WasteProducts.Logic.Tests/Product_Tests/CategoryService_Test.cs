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
        private CategoryDB categoryDB;

        [SetUp]
        public void Init()
        {
            mockCategoryRepo = new Mock<ICategoryRepository>();
            selectedList = new List<CategoryDB>();

            mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CategoryProfile>();
            });

            mapper = new Mapper(mapConfig);

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

            mockCategoryRepo.Verify(m => m.Add(It.IsAny<CategoryDB>()), Times.Never);
        }
    }

    
}
