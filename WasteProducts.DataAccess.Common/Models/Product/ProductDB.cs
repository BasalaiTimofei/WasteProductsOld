using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Product;

namespace WasteProducts.DataAccess.Common.Models.Product
{
    public class ProductDB
    {
        public string Name { get; set; }
        public string Id { get; }
        public DateTime Created { get; }
        public DateTime Updated { get; }
        public Category Category { get; set; }
        public Barcode Barcode { get; set; }
        public double AvgMark { get; internal set; }
        public decimal Price { get; }
        public int RateCount { get; internal set; }
    }
}
