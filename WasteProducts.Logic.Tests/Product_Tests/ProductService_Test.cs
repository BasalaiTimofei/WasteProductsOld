using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Mappings.Products;
using WasteProducts.Logic.Services.Products;

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
        private CategoryDB categoryDB;
        private MapperConfiguration mapConfig;
        private Mapper mapper;
        private Mock<IProductRepository> mockProductRepository;
        private Mock<ICategoryRepository> mockCategoryRepository;
        private Mock<IBarcodeService> mockBarcodeService;
        private Category category;

        [SetUp]
        public void Init()
        {
            barcode = new Barcode
            {
                Id = Guid.NewGuid().ToString(),
                Code = "456731556",
                ProductName = "Some product"
            };
            barcodeDB = new BarcodeDB
            {
                Id = Guid.NewGuid().ToString(),
                Code = "456731556",
                ProductName = "Some product"
            };

            selectedList = new List<ProductDB>();

            mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDB>()
                    .ForMember(m => m.Created,
                        opt => opt.MapFrom(p => p.Name != null ? DateTime.UtcNow : default(DateTime)))
                    .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?)null))
                    .ForMember(m => m.Barcode, opt => opt.Ignore())
                    .ReverseMap();
                cfg.AddProfile<CategoryProfile>();
            });

            mapper = new Mapper(mapConfig);

            mockProductRepository = new Mock<IProductRepository>();
            mockCategoryRepository = new Mock<ICategoryRepository>();
            mockBarcodeService = new Mock<IBarcodeService>();

            category = new Category
            {
                Id = new Guid().ToString(),
                Name = "Vegetables",
                Description = "Some description"
            };

            product = new Product { Id = new Guid().ToString(), Name = "Some name" };
            productDB = new ProductDB { Id = new Guid().ToString(), Name = "Some name" };
            categoryDB = new CategoryDB { Id = new Guid().ToString(), Name = "Some name" };
        }

        [Test]
        public void AddByName_InsertsNewProduct_CallsMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                productService.AddAsync(product.Name);

                mockProductRepository.Verify(m => m.AddAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void AddByName_DoesNotInsertNewProduct_DoesNotCallMethod_AddOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                productService.AddAsync(product.Name);

                mockProductRepository.Verify(m => m.AddAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void AddToCategory_AddsCategoryInProduct_CallsMethod_UpdateOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(categoryDB));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                productService.AddToCategoryAsync(product.Id, category.Id);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void AddToCategory_DoesNotAddCategoryInProduct_DoesNotCallMethod_UpdateOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            mockCategoryRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(categoryDB));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                productService.AddToCategoryAsync(product.Id, category.Id);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void GetById_GetsId_Returns_Product()
        {
            var id = new Guid().ToString();
            mockProductRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(productDB);

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                var result = productService.GetByIdAsync(id).Result;

                Assert.That(result, Is.TypeOf(typeof(Product)));
            }
        }

        [Test]
        public void GetById_GetsNullId_Returns_Null()
        {
            var id = new Guid().ToString();
            mockProductRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(productDB);

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                var result = productService.GetByIdAsync(null).Result;

                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public void GetByBarcode_GetsBarcode_Returns_Product()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                var result = productService.GetByBarcodeAsync(barcode).Result;

                Assert.That(result, Is.TypeOf(typeof(Product)));
            }
        }

        [Test]
        public void GetAll_Returns_GenericEnumerableCollection()
        {
            selectedList.Add(productDB);
            selectedList.Add(new ProductDB { Id = new Guid().ToString(), Name = "New Some Name" });
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                var result = productService.GetAllAsync().Result;

                Assert.That(result, Is.InstanceOf<IEnumerable<Product>>());
            }
        }

        [Test]
        public void GetByName_Returns_Product()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                var result = productService.GetByNameAsync(productName).Result;

                Assert.That(result, Is.TypeOf(typeof(Product)));
            }
        }

        [Test]
        public void GetByCategory_Returns_Products()
        {
            selectedList.Add(productDB);
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                var result = productService.GetByCategoryAsync(category).Result;

                Assert.That(result, Is.InstanceOf<IEnumerable<Product>>());
            }
        }

        [Test]
        public void Delete_DeletesProduct_CallsMethod_DeleteOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                productService.DeleteAsync(It.IsAny<string>());

                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void Delete_DoesNotDeleteProduct_DeleteOfRepositoryNeverCalled()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                productService.DeleteAsync(It.IsAny<string>());

                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void Update_CallsOnce()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                productService.UpdateAsync(product);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void Update_CallsNever()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mockCategoryRepository.Object, mockBarcodeService.Object, mapper))
            {
                productService.UpdateAsync(product);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }
    }
}
