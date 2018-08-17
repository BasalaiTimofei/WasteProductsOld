using System;
using System.ComponentModel;
using Moq;
using NUnit.Framework;
using NSubstitute;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private IProductService _product;
        private bool _added;
        private bool _notAdded;

        [SetUp]
        public void Init()
        {
            _product = Substitute.For<IProductService>();
            _added = true;
            _notAdded = false;
        }

        //[Test]
        //public void AddingProductByBarcore_Added()
        //{
        //    var barcode = new Barcode
        //    {
        //        barcode = 65478,
        //    };
        //    _product.AddByBarcode(barcode).Returns(_added);
        //}
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
            _product.AddByName(name).Returns(_added);

            Assert.AreEqual(name, _added);
        }
        public class Barcode
        {
            public int barcode { get; set; }

        }
    }
}
