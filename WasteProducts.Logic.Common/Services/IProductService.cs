using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Product;

namespace WasteProducts.Logic.Common.Services
{
    public interface IProductService
    {
        void AddByBarcode(Barcode barcode);

        void AddByName(string name);

        void DeleteByBarcode(Barcode barcode);

        void DeleteByName(string name);

        void AddCategory(Category category);

        void RemoveCategory(Category category);

        void SetPrice(decimal price);

        void AddAvgMark(int avgMark);

        void Hide(Product product);

        void Reveal(Product product);

        bool IsHidden(Product product);
    }
}
