using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WasteProducts.Logic.Common.Services.Groups
{
    public interface IGropService
    {
        // Создать группу
        void Create(string nameGroup, bool openGroup, User user/*, bool settings*/);

        //Login/Phone number/id/...
        void AddPerson(string login);

        void AddProduct(string nameProduct);
        
        // Поиск Группы
        void FindGrop(string nameGroup);

        // Поиск по заметкам
        void FindInGroup(string str);

        // Выйти из группы
        void Leave(User user, Group group);

        // Вступить в группу
        void Join(User user, Group group);

        // Пока сложно представляю реализовать
        string Chate(string text, User user, Group group);
    }
}
