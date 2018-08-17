using System;
using System.ComponentModel;
using Moq;
using NUnit.Framework;
using NSubstitute;
using WasteProducts.DataAccess.Common.Repositories;

namespace WasteProducts.Logic.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        class BarcodeInfo
        {
            private int barcode = 654298;

            public int GetInfo(int barcode)
            {
                return barcode;
            }
        }
        [Test]
        public void AddingProductByBarcore()
        {
            bool productAdded;
            int barcode = 654298;

            var product = Substitute.For<IProductRepository>();
            Assert.AreEqual(product.AddBy));
        }
    }
}
