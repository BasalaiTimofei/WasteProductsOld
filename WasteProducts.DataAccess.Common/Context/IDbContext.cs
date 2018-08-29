using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Context
{
    public interface IDbContext :IDisposable
    {
        Database Database { get; }
    }
}
