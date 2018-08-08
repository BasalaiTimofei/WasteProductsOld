using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models.Product
{
    public enum Category
    {

    }
    public class Product
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public Barcode Barcode { get; set; }
        public double AvgMark { get; internal set; }
        public decimal Price { get; set; }

        //для вычисления средней оценки нам понадобится количество оценивших данный продукт
        public int RateCount { get; internal set; } 
    }
}
