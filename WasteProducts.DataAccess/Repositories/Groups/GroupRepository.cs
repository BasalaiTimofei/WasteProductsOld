using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using WasteProducts.DataAccess.Common.Models.Groups;

namespace WasteProducts.DataAccess.Repositories.Groups
{
    class GroupRepository : IGroupRepository<GroupDB>
    {
        public void Create(GroupDB item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupDB> Find(Func<GroupDB, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public GroupDB Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupDB> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(GroupDB item)
        {
            throw new NotImplementedException();
        }
    }
}
