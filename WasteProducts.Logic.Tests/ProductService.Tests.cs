using System;
using System.ComponentModel;
using Moq;
using NUnit.Framework;
using NSubstitute;
using FluentAssertions;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;

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
//here, please consider to analyze the many unit tests.
    #endregion

    [TestFixture]
    public class ProductServiceTests
    {
        private IProductService _productSrvc;
        private Barcode _barcode;
        private bool _added;
        private bool _notAdded;

        [SetUp]
        public void Init()
        {
            _barcode = new Barcode { Code = "125478569", Brand = "Mars", Country = "Russia", Id = "51515" };
            _productSrvc = Substitute.For<IProductService>();
            _added = true;
            _notAdded = false;
        }

        [Test]
        public void AddingProductByBarcore_Added()
        {
            var isSuccess = _productSrvc.AddByBarcode(_barcode);
            
            Assert.That(isSuccess);
        }

        [Test]
        public void AddingProductByBarcore_IfBarcodesAreEqualShouldNotBeAdded()
        {
            var barcode1 = new Barcode();
            var barcode2 = new Barcode();
            
            var isSuccess = _productSrvc.AddByBarcode(barcode1);

            barcode1.Should().BeSameAs(barcode2);
            isSuccess.Should().BeFalse();
        }

        [Test]
        public void AddingProductByBarcore_BarcodeIsNotNull()
        {
            _barcode.Should().NotBe(null);
            var isSuccess = _productSrvc.AddByBarcode(_barcode);

            isSuccess.Should().BeTrue();
        }

        [Test]
        public void AddingProductByName_SuccessfullyAddedProduct()
        {
            var name = "Choco";

            var result = _productSrvc.AddByName(name);

            name.Should().NotBe(null);
            result.Should().Be(_added);
        }

        [Test]
        public void AddingProductByName_WasNotAddedProduct()
        {
            var chocolate = "Chocolate";
            var goal = _productSrvc.AddByName(chocolate);

            goal.Should().BeFalse();
        }

        [Test]
        public void AddingProductByName_NameIsNull()
        {
            var goal = _productSrvc.AddByName(null);

            Assert.IsTrue(goal);
        }
    }
}
