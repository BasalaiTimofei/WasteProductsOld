using System;
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
        public void Add_InsertNewCategory_ReturnsGuidId()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Add(It.IsAny<string>());

            Guid.TryParse(result.Result, out var guidId);

            Assert.That(guidId, Is.InstanceOf<Guid>());
        }

        [Test]
        public void Add_InsertedCategoryExists_ReturnsNull()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Add(It.IsAny<string>());

            Assert.That(result, Is.TypeOf(typeof(Task<string>)));
        }

        [Test]
        public void Add_InsertNewCategory_AddAsyncMethodOfRepoIsCalledOnce()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.Add(It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.AddAsync(It.IsAny<CategoryDB>()), Times.Once);
        }

        [Test]
        public void Add_InsertedCategoryExists_AddAsyncMethodOfRepoIsNeverCalled()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.Add(It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.AddAsync(It.IsAny<CategoryDB>()), Times.Never);
        }

        [Test]
        public void AddRange_InsertAtLeastOneNewCategory_ReturnsIds()
        {
            mockCategoryRepo.Setup(repo => repo.SelectAllAsync())
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.AddRange(names);

            Assert.That(result.Result, Is.InstanceOf<IEnumerable<string>>());
        }

        [Test]
        public void AddRange_InsertTwoNewCategories_AddRangeAsyncMethodOfRepoIsCalledOnce()
        {
            mockCategoryRepo.Setup(repo => repo.SelectAllAsync())
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.AddRange(names);

            mockCategoryRepo.Verify(m => m.AddRangeAsync(It.IsAny<IEnumerable<CategoryDB>>()), Times.Once);
        }

        [Test]
        public void AddRange_InsertAllExistingCategories_ReturnsNullAndAddRangeAsyncMethodOfRepoIsNeverCalled()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectAllAsync())
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.AddRange(names);

            Assert.That(result, Is.Null);
            mockCategoryRepo.Verify(m => m.AddRangeAsync(It.IsAny<IEnumerable<CategoryDB>>()), Times.Never);
        }

        [Test]
        public void Update_CategoryWasNotFound_ReturnsNull()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Update(It.IsAny<Category>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Update_CategoryWasFound_ReturnsTask()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Update(It.IsAny<Category>());

            Assert.That(result, Is.InstanceOf<Task>());
        }

        [Test]
        public void GetAll_GivesAllCategories_ReturnsEnumberable()
        {
            mockCategoryRepo.Setup(repo => repo.SelectAllAsync())
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.GetAll();

            Assert.That(result.Result, Is.InstanceOf<IEnumerable<Category>>());
        }

        [Test]
        public void GetAll_GivesAllCategories_GetAllAsyncMethodOfRepoIsCalledOnce()
        {
            mockCategoryRepo.Setup(repo => repo.SelectAllAsync())
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.GetAll();

            mockCategoryRepo.Verify(m => m.SelectAllAsync(), Times.Once);
        }

        [Test]
        public void GetById_GivesCategoryById_GetByIdAsyncMethodOfRepoIsCalledOnce()
        {
            var id = Guid.NewGuid().ToString();
            mockCategoryRepo.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(categoryDB);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.GetById(id);

            mockCategoryRepo.Verify(m => m.GetByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetById_GivesCategoryById_ReturnsCategory()
        {
            var id = Guid.NewGuid().ToString();
            mockCategoryRepo.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(categoryDB);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.GetById(id).Result;

            Assert.That(result, Is.InstanceOf<Category>());
        }

        [Test]
        public void GetByName_GivesCategoryByName_GetByNameAsyncMethodOfRepoIsCalledOnce()
        {
            mockCategoryRepo.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(categoryDB);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.GetByName(It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.GetByNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetByName_GivesCategoryByName_ReturnsCategory()
        {
            mockCategoryRepo.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(categoryDB);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.GetByName(It.IsAny<string>()).Result;

            Assert.That(result, Is.InstanceOf<Category>());
        }

        [Test]
        public void Delete_CategoryNotFound_ReturnsNull()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Delete(It.IsAny<string>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Delete_CategoryWasFound_ReturnsTask()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            var result = categoryService.Delete(It.IsAny<string>());

            Assert.That(result, Is.InstanceOf<Task>());
        }

        [Test]
        public void Delete_CategoryNotFound_DeleteMethodOfRepoIsNeverCalled()
        {
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.Delete(It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.DeleteAsync(It.IsAny<CategoryDB>()), Times.Never);
        }

        [Test]
        public void Delete_CategoryWasFound_DeleteMethodOfRepoIsCalledOnce()
        {
            selectedList.Add(categoryDB);
            mockCategoryRepo.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<CategoryDB>>()))
                .ReturnsAsync(selectedList);

            var categoryService = new CategoryService(mockCategoryRepo.Object, mapper);
            categoryService.Delete(It.IsAny<string>());

            mockCategoryRepo.Verify(m => m.DeleteAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
