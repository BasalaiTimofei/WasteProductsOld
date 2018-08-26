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
        private List<ProductDB> list;


        [SetUp]
        public void Init()
        {
            _barcode = new Barcode();
            _repo = new Mock<IProductRepository>();

            //var barcConf = new MapperConfiguration(option => option.CreateMap<Barcode, BarcodeDB>()
            //    .ForMember(s => s.Brend, d => d.Ignore())
            //    .ForMember(s => s.Modified, d => d.Ignore())
            //    .ForMember(s => s.Created, d => d.Ignore()));
            list = new List<ProductDB>();
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
            var barc = new Barcode
            {
                Code = "45376896782",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(list);
            
            var result = _productSrvc.AddByBarcode(barc);

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
            var barc1 = new Barcode
            {
                Code = "45376896782",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var r = _repo.Object;

            _repo.Setup(repo => repo.SelectWhere(It.IsAny<Predicate<ProductDB>>()))
                .Returns(list);
            var d = r.SelectWhere(db => _productSrvc.AddByBarcode(barc1));
           

            Assert.That(d != null);
            Assert.AreEqual(list.Count, d.Count());
            _repo.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Once);

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
            var goal = _productSrvc.AddByName(null);

            goal.Should().BeTrue();
        }

        [Test]
        public void AddingProductByName_SuccessfullyAddedProduct()
        {
            var name = "Choco";

            var result = _productSrvc.AddByName(name);
           
            result.Should().Be(true);
        }

        [Test]
        public void AddingProductByName_WasNotAddedProduct()
        {
            var chocolate = "Chocolate";
            var goal = _productSrvc.AddByName(chocolate);

            goal.Should().BeFalse();
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
            var result = _productSrvc.DeleteByBarcode(_barcode);

            _barcode.Should().NotBe(null);
            result.Should().BeTrue();
        }
        [Test]
        public void DeletingProductByName_IfProductsNamesAreSame()
        {
            var barc1 = new Barcode
            {
                Code = "45376896782",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var barc2 = new Barcode
            {
                Code = "863863896745",
                Id = Guid.NewGuid().ToString(),
                ProductName = "Red Cherry"
            };
            var prod1 = new Product { Name = "Red Cherry", Barcode = barc1 };
            var prod2 = new Product { Name = "Red Cherry", Barcode = barc2 };

            barc1.ProductName.Should().BeEquivalentTo(barc2.ProductName);
            prod1.Name.Should().BeSameAs(barc1.ProductName);
            prod2.Name.Should().BeSameAs(barc2.ProductName);
            barc1.Code.Should().NotBeSameAs(barc2.Code);
            barc1.Id.Should().NotBeSameAs(barc2.Id);

            var result = _productSrvc.DeleteByName(prod1.Name);
            prod1.Name.Should().BeSameAs(prod2.Name);

            result.Should().BeTrue();
        }
        [Test]
        public void DeletingProductByName_SuccessfullyDeletedProduct()
        {
            var prodGummy = "My favorite gummy";

            var result = _productSrvc.DeleteByName(prodGummy);

            prodGummy.Should().BeEquivalentTo("MY FAVORITE GUMMY")
                .And.NotBeNullOrWhiteSpace()
                .And.NotBeEmpty();
            result.Should().Be(true);
        }
    }
}