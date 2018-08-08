using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models
{
    public interface ISearchQuery
    {
        string Query { get; set; }
        string[] SearchableFields { get; set; }
    }
}
