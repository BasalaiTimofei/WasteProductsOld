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
    class GroupBordRepository : IGroupRepository<GroupBordDB>
    {
        WasteContext db;

        public GroupBordRepository(WasteContext context)
        {
            db = context;
        }

        public void Create(GroupBordDB item)
        {
            db.GroupBordDBs.Add(item);
        }

        public void Update(GroupBordDB item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            GroupBordDB group = db.GroupBordDBs.Find(id);
            if (group != null)
                db.GroupBordDBs.Remove(group);
        }

        public IEnumerable<GroupBordDB> Find(Func<GroupBordDB, bool> predicate)
        {
            return db.GroupBordDBs.Where(predicate).ToList();
        }

        public GroupBordDB Get(int id)
        {
            return db.GroupBordDBs.Find(id);
        }

        public IEnumerable<GroupBordDB> GetAll()
        {
            return db.GroupBordDBs;
        }

    }
}
