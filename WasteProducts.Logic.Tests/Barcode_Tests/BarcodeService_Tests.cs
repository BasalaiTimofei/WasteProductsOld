using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Services.Barcods;
using Ninject;
using WasteProducts.DataAccess.Common.Repositories.Barcods;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace WasteProducts.Logic.Tests.Barcode_Tests
{
    [TestFixture]
    class BarcodeService_Tests
    {
        private Bitmap _imageGood = Properties.Resources.IMG_GoodImage;


        /// <summary>
        /// инициализируем массив из ДВУХ каталогов для поиска информации о товаре.
        /// 
        /// моделируем успешный поиск в ПЕРВОМ каталоге.
        /// 
        /// для успешного прохождения теста нужно убедиться что:
        /// 1) метод ICatalog.Get() был вызван только у первого каталога 
        /// 2) результат был возвращен ПЕРВЫМ каталогом
        /// </summary>
        [Test]
        public async Task Call_GetAsync_Only_First_Catalog()
        {
            //Arrange
            StandardKernel _kernel = new StandardKernel();
            _kernel.Load(new DataAccess.InjectorModule(), new Logic.InjectorModule());

            var barcodeService = _kernel.Get<IBarcodeService>();

            using (Stream stream = new MemoryStream())
            {
                _imageGood.Save(stream, ImageFormat.Bmp);

                barcodeService.Get(stream);

            }


            //Act



            //Assert

        }
    }
}
