using System.Collections.Generic;

namespace WasteProducts.Logic.Common.Services
{
    public interface IGropUserManagerService
    {
        void AddUser<T>(T item);
        void DeleteUser<T>(T item);
    }
}
