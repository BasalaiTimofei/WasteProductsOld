using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Product;
using WasteProducts.Logic.Common.Models.Product;

namespace WasteProducts.DataAccess.Common.Repositories
{
    /// <summary>
    /// Interface for the ProductRepository. Has an inheritance branch from IDisposable.
    /// </summary>
    public interface IProductRepository : IDisposable
    {
        IEnumerable<ProductDB> GetProducts();
        ProductDB GetById(string id);
        void Add(ProductDB product);
        void Update(ProductDB product);
        void DeleteById(string id);
        void Save();
    }
}
