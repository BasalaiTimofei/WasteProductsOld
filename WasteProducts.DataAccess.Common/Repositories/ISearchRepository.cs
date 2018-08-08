using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Repositories
{
    interface ISearchRepository
    {
        T Get<T>(int Id);
        void Insert<T>(T obj);
        void Update<T>(T obj);
        void Delete<T>(T obj);
        IEnumerable<T> GetAll<T>();
    }
}
