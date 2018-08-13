using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.DataAccess.Contexts;
using System.Data.Entity;

namespace WasteProducts.DataAccess.Repositories.Groups
{
    class ProductBordDBRepository : IGroupRepository<ProductBordDB>
    {
        WasteContext db;

        public ProductBordDBRepository(WasteContext context)
        {
            db = context;
        }

        public void Create(ProductBordDB item)
        {
            db.ProductBordDBs.Add(item);
        }

        public void Update(ProductBordDB item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ProductBordDB group = db.ProductBordDBs.Find(id);
            if (group != null)
                db.ProductBordDBs.Remove(group);
        }

        public IEnumerable<ProductBordDB> Find(Func<ProductBordDB, bool> predicate)
        {
            return db.ProductBordDBs.Where(predicate).ToList();
        }

        public ProductBordDB Get(int id)
        {
            return db.ProductBordDBs.Find(id);
        }

        public IEnumerable<ProductBordDB> GetAll()
        {
            return db.ProductBordDBs;
        }

    }
}
