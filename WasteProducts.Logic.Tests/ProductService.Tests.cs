using System;
using System.ComponentModel;
using Moq;
using NUnit.Framework;
using NSubstitute;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Tests
{
    //What is NSubstitute? http://nsubstitute.github.io/
    //NSubstitute is designed as a friendly substitute for .NET mocking libraries.
    //It is an attempt to satisfy our craving for a mocking library with a succinct syntax that helps 
    //us keep the focus on the intention of our tests, rather than on the configuration of our test doubles.
    //We've tried to make the most frequently required operations obvious and easy to use, keeping less usual
    //scenarios discoverable and accessible, and all the while maintaining as much natural language as possible.

    [TestFixture]
    public class ProductServiceTests
    {
        private IProductService _product;
        private IBarcodeService _barcode;
        private bool _added;
        private bool _notAdded;

        [SetUp]
        public void Init()
        {
            _product = Substitute.For<IProductService>();
            _barcode = Substitute.For<IBarcodeService>();
            _added = true;
            _notAdded = false;
        }

        [Test]
        public void AddingProductByBarcore_Added()
        {
            var barcode = new Barcode
            {
                Code = "125478569",
            };
            var isSuccess = _product.AddByBarcode(barcode);
            isSuccess.Returns(_added);

            Assert.That(isSuccess);
            Assert.That(!isSuccess);
        }
        [Test]
        public void AddingProductByName_SuccessfullyAddedProduct()
        {
            var name = "Chocolate";
            _product.AddByName(name).Returns(_added);

            Assert.AreEqual(name, _added);
        }

        [Test]
        public void AddingProductByName_WasNotAddedProduct()
        {
            var name = "Chocolate";
            var goal = _product.AddByName(name);
            goal.Returns(_notAdded);

            Assert.IsFalse(goal);
        }
    }
}
