using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Product;

namespace WasteProducts.Logic.Common.Services
{
    public interface ICategoryService
    {
        bool Add(string name);

        bool AddRange(IEnumerable<string> names);

        Category Search(string name);

        bool Delete(string name);

        bool DeleteRange(IEnumerable<string> names);
    }
}
