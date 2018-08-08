using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    class GroupDB
    {
        public string Name { get; set; }

        public List<User> ListUsers;

        public List<Product> ListProducts;

        // Информация (может и не нужна) которую лучше вынести в другой класс(Для удобства)
        public GroupInfo Info;
    }