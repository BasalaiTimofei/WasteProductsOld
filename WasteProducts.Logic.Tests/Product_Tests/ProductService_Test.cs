using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Mappings;
using WasteProducts.Logic.Mappings.Products;
using WasteProducts.Logic.Services;
using WasteProducts.Logic.Services.Products;

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

            category = new Category
            {
                Name = "Vegetables",
                Description = "Some description",
                Products = new List<Product>()
            };

            product = new Product { Id = new Guid().ToString(), Name = "Some name" };
            productDB = new ProductDB { Id = new Guid().ToString(), Name = "Some name" };
        }

        [Test]
        public void Add_InsertsNewProduct_Returns_True()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.Add(product, out var addedProduct);

                Assert.That(result, Is.EqualTo(true));
            }
        }

        [Test]
        public void Add_DoesNotInsertNewProduct_Returns_False()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.Add(product, out var addedProduct);

                Assert.That(result, Is.EqualTo(false));
            }
        }

        [Test]
        public void Add_DoesNotInsertNullProduct_Throws_NullReferenceException()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                Assert.Throws<NullReferenceException>(() => productService.Add((Product)null, out var addedProduct));
            }
        }

        [Test]
        public void Add_InsertsNewProduct_CallsMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Add(product, out var addedProduct);

                mockProductRepository.Verify(m => m.AddAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void Add_DoesNotInsertNewProduct_DoesNotCallMethod_AddOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Add(product, out var addedProduct);

                mockProductRepository.Verify(m => m.AddAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void Add_DoesNotInsertNullProduct_DoesNotCallMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                Assert.Throws<NullReferenceException>(() => productService.Add((Product)null, out var addedProduct));
                mockProductRepository.Verify(m => m.AddAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void AddByBarcode_InsertsNewProduct_Returns_True()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.Add(barcode, out var addedProduct);

                Assert.That(result, Is.EqualTo(true));
            }
        }

        [Test]
        public void AddByBarcode_DoesNotInsertNewProduct_Returns_False()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.Add(barcode, out var addedProduct);

                Assert.That(result, Is.EqualTo(false));
            }
        }

        [Test]
        public void AddByBarcore_BarcodeIsNotNull()
        {
            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var isSuccess = productService.Add(barcode, out var addedProduct);

                isSuccess.Should().BeTrue().And.Should().NotBe(null);
            }
        }

        [Test]
        public void AddByBarcode_InsertsNewProduct_CallsMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Add(barcode, out var addedProduct);

                mockProductRepository.Verify(m => m.AddAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void AddByBarcode_DoesNotInsertNewProduct_DoesNotCallMethod_AddOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Add(barcode, out var addedProduct);

                mockProductRepository.Verify(m => m.AddAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void AddByName_InsertsNewProduct_Returns_True()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.Add(productName, out var addedProduct);

                Assert.That(result, Is.EqualTo(true));
            }
        }

        [Test]
        public void AddByName_DoesNotInsertNewProduct_Returns_False()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.Add(It.IsAny<string>(), out var addedProduct);

                Assert.That(result, Is.EqualTo(false));
            }
        }

        [Test]
        public void AddName_NameIsNull()
        {
            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Add((string)null, out var addedProduct);
                mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                    .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void AddByName_InsertsNewProduct_CallsMethod_AddOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Add(productName, out var addedProduct);

                mockProductRepository.Verify(m => m.AddAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void AddByName_DoesNotInsertNewProduct_DoesNotCallMethod_AddOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Add(It.IsAny<string>(), out var addedProduct);

                mockProductRepository.Verify(m => m.AddAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void AddCategoty_AddsCategoryInProductWithoutCategory_Returns_True()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.AddCategory(product, category);

                Assert.That(result, Is.EqualTo(true));
            }
        }

        [Test]
        public void AddCategoty_DoesNotAddCategoryInProductWithCategory_Returns_False()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.AddCategory(product, category);

                Assert.That(result, Is.EqualTo(false));
            }
        }

        [Test]
        public void AddCategoty_AddsCategoryInProductWithoutCategory_CallsMethod_UpdateOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.AddCategory(product, category);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void AddCategoty_DoesNotAddCategoryInProductWithCategory_DoesNotCallMethod_UpdateOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.AddCategory(product, category);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void GetById_GetsId_Returns_Product()
        {
            var id = new Guid().ToString();
            mockProductRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(productDB);

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.GetById(id);

                Assert.That(result, Is.InstanceOf(typeof(Product)));
            }
        }

        [Test]
        public void GetById_GetsId_Returns_Null()
        {
            var id = new Guid().ToString();
            mockProductRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(productDB);

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.GetById(null);

                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public void GetByBarcode_GetsBarcode_Returns_Product()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.GetByBarcode(barcode);

                Assert.That(result, Is.InstanceOf(typeof(Product)));
            }
        }

        [Test]
        public void GetAll_GetsNothing_Returns_GenericEnumerableCollection()
        {
            selectedList.Add(productDB);
            selectedList.Add(new ProductDB { Id = new Guid().ToString(), Name = "New Some Name" });
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.GetAll();

                Assert.That(result, Is.InstanceOf<IEnumerable<Product>>());
            }
        }

        [Test]
        public void GetByName_GetProductName_Returns_Product()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.GetByName(productName);

                Assert.That(result, Is.InstanceOf(typeof(Product)));
            }
        }

        [Test]
        public void DeleteByBarcode_DoNotDeletesProduct_Returns_False()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.Delete(barcode);

                Assert.That(result, Is.EqualTo(false));
            }
        }

        [Test]
        public void DeleteByBarcode_DeletesProduct_CallsMethod_DeleteOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Delete(barcode);

                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void DeleteByBarcode_DoesNotDeleteProduct_DoesNotCallMethod_DeleteOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Delete(barcode);

                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void DeleteByName_DeletesProduct_Returns_True()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.Delete(It.IsAny<string>());

                result.Should().BeTrue();
            }
        }

        [Test]
        public void DeleteByName_DoesNotDeleteProduct_Returns_False()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                var result = productService.Delete(It.IsAny<string>());

                Assert.That(result, Is.EqualTo(false));
            }
        }

        [Test]
        public void DeletetByName_IfNameIsEmpty_CalledNever()
        {
            var prod = new ProductDB { Name = "" };
            selectedList.Add(prod);

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Delete(prod.Name);

                mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                    .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

                prod.Name.Should().BeEmpty();

                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void DeleteByName_IfNameIsNull_CalledNever()
        {
            var prod = new ProductDB { Name = null };
            selectedList.Add(prod);

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Delete(prod.Name);

                mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                    .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

                prod.Name.Should().BeNull();
                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void DeleteByName_DeletesProduct_CallsMethod_DeleteOfRepository()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Delete(It.IsAny<string>());

                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void DeleteByName_DoNotDeleteProduct_DoesNotCallMethod_DeleteOfRepository()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Delete(It.IsAny<string>());

                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        
        [Test]
        public void RemoveCategory_Removed_ReturnsTrue()
        {
            selectedList.Add(new ProductDB());
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.RemoveCategory(product, new Category()).Should().BeTrue();
            }
        }

        [Test]
        public void RemoveCategory_NotRemoved_ReturnsFalse()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.RemoveCategory(product, new Category()).Should().BeFalse();
            }
        }

        [Test]
        public void RemoveCategory_UpdateOfRepositoryIsNeverCalled()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.RemoveCategory(product, category);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        
        [Test]
        public void SetDescription_Success()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.SetComposition(product, "Оч крутой кисель");

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void SetDescription_NotSuccess()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.SetComposition(product, "Оч крутой кисель");

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void SetDescription_IfDescriptionIsNull()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.SetComposition(product, null);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void Update_CallsOnce()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Update(product);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void Update_CallsNever()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Update(product);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }

        [Test]
        public void Update_IfProductIsInDb()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Update(product).Should().BeTrue();
            }
        }

        [Test]
        public void Update_IfProductIsNotInDb()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Update(product).Should().BeFalse();
            }
        }

        [Test]
        public void Delete_IfProductIsInDb()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Delete(product).Should().BeTrue();
            }
        }

        [Test]
        public void Delete_IfProductIsNotInDb()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Delete(product).Should().BeFalse();
            }
        }

        [Test]
        public void Delete_CallsOnce()
        {
            selectedList.Add(productDB);
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Update(product);

                mockProductRepository.Verify(m => m.UpdateAsync(It.IsAny<ProductDB>()), Times.Once);
            }
        }

        [Test]
        public void Delete_CallsNever()
        {
            mockProductRepository.Setup(repo => repo.SelectWhereAsync(It.IsAny<Predicate<ProductDB>>()))
                .Returns(Task.FromResult((IEnumerable<ProductDB>)selectedList));

            using (var productService = new ProductService(mockProductRepository.Object, mapper))
            {
                productService.Update(product);

                mockProductRepository.Verify(m => m.DeleteAsync(It.IsAny<ProductDB>()), Times.Never);
            }
        }
    }
}
