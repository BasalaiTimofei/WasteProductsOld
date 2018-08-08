using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories.Groups
{
    public interface IGroupsRepository
    {
        object Get();
        void Add();
        void Update();
        void Delete();
    }
}
