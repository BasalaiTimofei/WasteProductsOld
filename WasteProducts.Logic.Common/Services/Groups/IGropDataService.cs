using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services
{
    public interface IGropDataService
    {
        T Get<T>(int? id);
        IEnumerable<T> GetAll<T>();
    }
}
