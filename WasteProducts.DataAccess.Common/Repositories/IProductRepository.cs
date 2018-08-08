using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Product;

namespace WasteProducts.DataAccess.Common.Repositories
{
    public interface IProductRepository : IDisposable
    {
        IEnumerable<Product> GetProducts();
        Product GetById(string id);
        void Add(Product product);
        void Update(Product product);
        void DeleteById(string id);
        void Save();
    }
}
