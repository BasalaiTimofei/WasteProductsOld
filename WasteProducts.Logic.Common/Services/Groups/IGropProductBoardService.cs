using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services
{
    public interface IGropProductBoardService
    {
        void Create<T>(T item);
        void Update<T>(T item);
        void Delete<T>(int? id);
        void Dispose();
    }
}
