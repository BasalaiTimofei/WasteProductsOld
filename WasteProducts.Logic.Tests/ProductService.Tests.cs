using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using NSubstitute;
using FluentAssertions;
using NSubstitute.Extensions;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Services;
using AutoMapper;
using NUnit.Framework.Internal;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.Logic.Mappings;

namespace WasteProducts.Logic.Tests
{
    #region NSubstitute description
    //What is NSubstitute? http://nsubstitute.github.io/
    //NSubstitute is designed as a friendly substitute for .NET mocking libraries.
    //It is an attempt to satisfy our craving for a mocking library with a succinct syntax that helps 
    //us keep the focus on the intention of our tests, rather than on the configuration of our test doubles.
    //We've tried to make the most frequently required operations obvious and easy to use, keeping less usual
    //scenarios discoverable and accessible, and all the while maintaining as much natural language as possible.
    #endregion

    #region FluentAssertions descreiption
    //https://fluentassertions.com/documentation/
    //As you may have noticed, the purpose of this open-source project is to not only be the best 
    //assertion framework in the.NET realm, but to also demonstrate high-quality code. We heavily 
    //practice Test Driven Development and one of the promises TDD makes is that unit tests can be
    //treated as your API’s documentation. So although you are free to go through the many examples
    //here, please consider to analyze the many unit tests. (developer's comment)
    #endregion

    [TestFixture]
    public class ProductServiceTests
    {
        private Barcode _barcode;
        private IProductService _productSrvc;
        private Mock<IProductRepository> _repo;
        private IMapper _mapper;
        private List<ProductDB> listDb;
        private List<Product> list;



        [SetUp]
        public void Init()
        {
            _barcode = new Barcode();
            _repo = new Mock<IProductRepository>();
            
            listDb = new List<ProductDB>();
            var prodConf = new MapperConfiguration(option => option.CreateMap<Product, ProductDB>()
                .ForMember(m => m.Created,
                    opt => opt.MapFrom(p => p.Name != null ? DateTime.UtcNow : default(DateTime)))
                .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?)null))
                .ForMember(s => s.Category, d => d.Ignore())
                .ForMember(s => s.Barcode, opt => opt.Ignore())
                .ReverseMap());
            
            var prodMapper = new Mapper(prodConf);
            _productSrvc = new ProductService(_repo.Object, prodMapper);

        }

        [TearDown]
        public void DropInit()
        {
            Mapper.Reset();
        }

        [Test]
        public void AddByBarcode_InsertNewProduct_callsMethod_AddOfRepository()
        {
            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(listDb);
            
            _productSrvc.AddByBarcode(_barcode);

            _repo.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Once);
        }

        [Test]
        public void AddingProductByBarcore_BarcodeIsNotNull()
        {
            var isSuccess = _productSrvc.AddByBarcode(_barcode);
            
            isSuccess.Should().BeTrue();
        }

        [Test]
        public void AddingProductByBarcore_ReturnsProduct()
        {
            var barc = new Barcode
            {
                Code = "45376896782",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var r = _repo.Object;

            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(listDb);
            var d = r.SelectWhere(db => _productSrvc.AddByBarcode(barc));

            Assert.That(d != null);
            Assert.AreEqual(listDb.Count, d.Count());
        }

        [Test]
        public void AddingProductByBarcore_IfBarcodesAreEqualShouldNotBeAdded()
        {
            var barc1 = new Barcode();
            var barc2 = new Barcode();

            var result1 = _productSrvc.AddByBarcode(barc1);
            var result2 = _productSrvc.AddByBarcode(barc2);

            Assert.AreNotSame(result1,result2);
        }

        [Test]
        public void AddingProductByBarcore_SuccessfullyAdded()
        {
            var barc = new Barcode();

            var isSuccess = _productSrvc.AddByBarcode(barc);
           
            Assert.That(isSuccess);
        }

        [Test]
        public void AddingProductByName_NameIsNull()
        {
            _productSrvc.AddByName(null);

            _repo.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void AddingProductByName_SuccessfullyAddedProduct()
        {
            var name = "Choco";

            var result = _productSrvc.AddByName(name);
           
            result.Should().Be(true);
        }

        [Test]
        public void AddingProductByName_IfProductsNameMeetSameNameInAnotherBarcode()
        {
            var barc1 = new Barcode();
            var barc2 = new Barcode();

            var prod1 = new Product
            {
                Name = "ice cream"
            };
            var prod2 = new Product
            {
                Name = "ice cream with vanilla flavour"
            };

            barc1.Code.Should().BeSameAs(barc2.Code);
            barc1.Id.Should().BeSameAs(barc2.Id);
            prod2.Name.Should().Contain("ice")
                .And.NotBeSameAs(prod1.Name);

            var result = _productSrvc.AddByName(prod2.Name);

            result.Should().BeTrue();
        }

        [Test]
        public void DeletingProductByBarcore_BarcodeIsNotNull_AndProductSuccessfullyDeleted()
        {
            var prod = new ProductDB();
            
            listDb.Add(prod);
            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(listDb);
            var result = _productSrvc.DeleteByBarcode(new Barcode());
            
            result.Should().BeTrue();
        }

        [Test]
        public void DeletingProductByBarcore_BarcodeIsNotNull_AndProductSuccessfullyDeleted_CalledOnce()
        {
            var prod = new ProductDB();
            var prod1 = new ProductDB();

            listDb.Add(prod);
            listDb.Add(prod1);
            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(listDb);
            _productSrvc.DeleteByBarcode(new Barcode());

            _repo.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Once);
        }
        [Test]
        public void DeletingProductByName_IfProductsNamesAreSame_ReturnsTrue()
        {
            var barc1 = new BarcodeDB
            {
                Code = "45376896782",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var barc2 = new BarcodeDB
            {
                Code = "863863896745",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var prod1 = new ProductDB { Name = "Red Cherry", Barcode = barc1 };
            var prod2 = new ProductDB { Name = "Red Cherry", Barcode = barc2 };

            listDb.Add(prod1);
            listDb.Add(prod2);
            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(listDb);

            var result = _productSrvc.DeleteByName(prod1.Name);

            prod1.Name.Should().BeSameAs(prod2.Name);

            result.Should().BeTrue();
        }

        [Test]
        public void DeletingProductByName_IfProductsNamesAreSame_ReturnsTrue_CalledOnce()
        {
            var barc1 = new BarcodeDB
            {
                Code = "45376896782",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var barc2 = new BarcodeDB
            {
                Code = "863863896745",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var prod1 = new ProductDB { Name = "Red Cherry", Barcode = barc1 };
            var prod2 = new ProductDB { Name = "Red Cherry", Barcode = barc2 };

            listDb.Add(prod1);
            listDb.Add(prod2);
            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(listDb);

            _productSrvc.DeleteByName(prod1.Name);

            prod1.Name.Should().BeSameAs(prod2.Name);

            _repo.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Once);
        }
        [Test]
        public void DeletingProductByName_SuccessfullyMarkedProduct()
        {
            var barc = new BarcodeDB
            {
                Code = "45376896782",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var prod = new ProductDB { Name = "Red Cherry", Barcode = barc };
            listDb.Add(prod);
            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(listDb);

            var result = _productSrvc.DeleteByName(prod.Name);

            prod.Name.Should().BeEquivalentTo("RED CHERRY")
                .And.NotBeEmpty();
            result.Should().Be(true);
        }

        [Test]
        public void DeleteProductByName_IfNameIsEmpty_CalledNever()
        {
            var prod = new ProductDB {Name = ""};
            listDb.Add(prod);

            _productSrvc.DeleteByName(prod.Name);
            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(listDb);
            prod.Name.Should().BeEmpty();

            _repo.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Never);
        }

        [Test]
        public void DeleteProductByName_IfNameIsNull_CalledNever()
        {
            var prod = new ProductDB { Name = null };
            listDb.Add(prod);

            _productSrvc.DeleteByName(prod.Name);
            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(listDb);

            prod.Name.Should().BeNull();
            _repo.Verify(m => m.Delete(It.IsAny<ProductDB>()), Times.Never);
        }
    }
}