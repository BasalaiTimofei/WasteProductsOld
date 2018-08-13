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
    class GroupUserDBRepository : IGroupRepository<GroupUserDB>
    {
        WasteContext db;

        public GroupUserDBRepository(WasteContext context)
        {
            db = context;
        }

        public void Create(GroupUserDB item)
        {
            db.GroupUserDBs.Add(item);
        }

        public void Update(GroupUserDB item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            GroupUserDB group = db.GroupUserDBs.Find(id);
            if (group != null)
                db.GroupUserDBs.Remove(group);
        }

        public IEnumerable<GroupUserDB> Find(Func<GroupUserDB, bool> predicate)
        {
            return db.GroupUserDBs.Where(predicate).ToList();
        }

        public GroupUserDB Get(int id)
        {
            return db.GroupUserDBs.Find(id);
        }

        public IEnumerable<GroupUserDB> GetAll()
        {
            return db.GroupUserDBs;
        }

    }
}
