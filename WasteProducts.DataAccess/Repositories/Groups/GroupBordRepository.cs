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
    class GroupBordRepository : IGroupRepository<GroupBoardDB>
    {
        WasteContext db;

        public GroupBordRepository(WasteContext context)
        {
            db = context;
        }

        public void Create(GroupBoardDB item)
        {
            db.GroupBordDBs.Add(item);
        }

        public void Update(GroupBoardDB item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            GroupBoardDB group = db.GroupBordDBs.Find(id);
            if (group != null)
                db.GroupBordDBs.Remove(group);
        }

        public IEnumerable<GroupBoardDB> Find(Func<GroupBoardDB, bool> predicate)
        {
            return db.GroupBordDBs.Where(predicate).ToList();
        }

        public GroupBoardDB Get(int id)
        {
            return db.GroupBordDBs.Find(id);
        }

        public IEnumerable<GroupBoardDB> GetAll()
        {
            return db.GroupBordDBs;
        }

    }
}
