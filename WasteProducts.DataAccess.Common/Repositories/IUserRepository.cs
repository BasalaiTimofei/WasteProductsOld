using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Repositories
{
    public interface IUserRepository
    {
        UserDB Select(int userID);

        IEnumerable<UserDB> SelectAll();

        // не знаю пока, как надо, такое сработало бы на EF или ADO.NET если все запросить, и потом предикатом
        // но это ведь глупость, лучше сразу в селект передать запрос
        // скажем так, это заготовка на будущее, потом изменится на нормальный код
        IEnumerable<UserDB> SelectWhere(Predicate<UserDB> predicate); 

        void Add(UserDB user);

        void Update(UserDB user);

        void Delete(UserDB user);
    }
}