using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services
{
    /// <summary>
    /// Product administration service
    /// </summary>
    public interface IGropProductBoardService
    {
        void Create<T>(T item);
        void Update<T>(T item);
        void Delete<T>(int? id);
    }
}
