﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Mappings;
using WasteProducts.Logic.Mappings.Products;
using WasteProducts.Logic.Services;
using WasteProducts.Logic.Services.Products;

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
        public void AddByName_InsertNewCategory_ReturnsGuidId()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Add(It.IsAny<string>());

            Guid.TryParse(result.Result, out var guidId);

            Assert.That(guidId, Is.InstanceOf<Guid>());
        }

        [Test]
        public void AddByName_InsertedCategoryExists_ReturnsNull()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Add(It.IsAny<string>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddByName_InsertNewCategory_AddAsyncMethodOfRepoIsCalledOnce()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.Add(It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.AddAsync(It.IsAny<CategoryDB>()), Times.Once);
        }

        [Test]
        public void AddByName_InsertedCategoryExists_AddMethodOfRepoIsNeverCalled()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.Add(It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.AddAsync(It.IsAny<CategoryDB>()), Times.Never);
        }

        //TODO Не сделан метод AddRangeAsync
        //[Test]
        //public void AddRange_InsertAtLeastOneNewCategory_ReturnsTrue()
        //{
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    var result = categoryService.AddRange(names);

        //    Assert.That(result, Is.EqualTo(true));
        //}

        //[Test]
        //public void AddRange_InsertTwoNewCategories_AddMethodOfRepoIsCalledTwice()
        //{
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    categoryService.AddRange(names);

        //    mockCategoryRepo.Verify(m => m.AddAsync(It.IsAny<CategoryDB>()), Times.Exactly(2));
        //}

        //[Test]
        //public void AddRange_InsertAllExistingCategories_ReturnsFalseAndAddMethodOfRepoIsNeverCalled()
        //{
        //    selectedList.Add(categoryDB);
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    var result = categoryService.AddRange(names);

        //    Assert.That(result, Is.EqualTo(false));
        //    mockCategoryRepo.Verify(m => m.AddAsync(It.IsAny<CategoryDB>()), Times.Never);
        //}

        [Test]
        public void GetAll_GetsAllCategories_ReturnsEnumberable()
        {
            mockCategoryRepo.Setup(repo => repo.SelectAllAsync())
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.GetAll();

            Assert.That(result.Result, Is.InstanceOf<IEnumerable<Category>>());
        }

        [Test]
        public void GetAll_GetsAllCategories_GetAllAsyncMethodOfRepoIsCalledOnce()
        {
            mockCategoryRepo.Setup(repo => repo.SelectAllAsync())
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.GetAll();

            mockCategoryRepo.Verify(m => m.SelectAllAsync(), Times.Once);
        }

        //[Test]
        //public void Get_CategoryNotFound_ReturnsNull()
        //{
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    var result = categoryService.Get(It.IsAny<string>());

        //    Assert.That(result, Is.Null);
        //}

        //[Test]
        //public void Get_CategoryWasFound_NamesAreTheSame()
        //{
        //    selectedList.Add(new CategoryDB { Name = "Meat" });
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    var result = categoryService.Get(It.IsAny<string>());

        //    Assert.That(result, Is.TypeOf(typeof(Category)));
        //    Assert.AreEqual("Meat", result.Name);
        //}

        //[Test]
        //public void SetDescription_CategoryNotFound_UpdateMethodIsNeverCalled()
        //{
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    categoryService.SetDescription(category, It.IsAny<string>());

        //    mockCategoryRepo.Verify(m => m.UpdateAsync(It.IsAny<CategoryDB>()), Times.Never);
        //}

        //[Test]
        //public void SetDescription_CategoryWasFound_DescriptionIsAddedAndUpdateMethodIsCalledOnce()
        //{
        //    selectedList.Add(categoryDB);
        //    var description = "Some description";
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    categoryService.SetDescription(category, description);

        //    Assert.AreEqual(selectedList[0].Description, description);
        //    mockCategoryRepo.Verify(m => m.UpdateAsync(It.IsAny<CategoryDB>()), Times.Once);
        //}

        //[Test]
        //public void Delete_CategoryNotFound_ReturnsFalse()
        //{
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    var result = categoryService.Delete(It.IsAny<string>());

        //    Assert.That(result, Is.EqualTo(false));
        //}

        //[Test]
        //public void Delete_CategoryWasFound_ReturnsTrue()
        //{
        //    selectedList.Add(categoryDB);
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    var result = categoryService.Delete(It.IsAny<string>());

        //    Assert.That(result, Is.EqualTo(true));
        //}

        //[Test]
        //public void Delete_CategoryNotFound_DeleteMethodOfRepoIsNeverCalled()
        //{
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    categoryService.Delete(It.IsAny<string>());

        //    mockCategoryRepo.Verify(m => m.DeleteAsync(It.IsAny<CategoryDB>()), Times.Never);
        //}

        //[Test]
        //public void Delete_CategoryWasFound_DeleteMethodOfRepoIsCalledOnce()
        //{
        //    selectedList.Add(categoryDB);
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    categoryService.Delete(It.IsAny<string>());

        //    mockCategoryRepo.Verify(m => m.DeleteAsync(It.IsAny<CategoryDB>()), Times.Once);
        //}

        //[Test]
        //public void DeleteRange_DeleteAtLeastOneExistingCategory_ReturnsTrue()
        //{
        //    selectedList.Add(categoryDB);
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    var result = categoryService.DeleteRange(names);

        //    Assert.That(result, Is.EqualTo(true));
        //}

        //[Test]
        //public void DeleteRange_DeleteTwoExistingCategories_DeleteMethodOfRepoIsCalledTwice()
        //{
        //    selectedList.Add(categoryDB);
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    categoryService.DeleteRange(names);

        //    mockCategoryRepo.Verify(m => m.DeleteAsync(It.IsAny<CategoryDB>()), Times.Exactly(2));
        //}

        //[Test]
        //public void DeleteRange_CategoriesWereNotFound_ReturnFalseAndDeleteMethodOfRepoNeverCalled()
        //{
        //    mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
        //        .Returns(Task.FromResult((IEnumerable<CategoryDB>)selectedList));

        //    var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
        //    var result = categoryService.DeleteRange(names);

        //    Assert.That(result, Is.EqualTo(false));
        //    mockCategoryRepo.Verify(m => m.DeleteAsync(It.IsAny<CategoryDB>()), Times.Never);
        //}
    }
}
