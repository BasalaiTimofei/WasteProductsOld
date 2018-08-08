using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Enums;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    public class UserDB
    {
        public int Id { get; }

        public string Login { get; }

        public string Email { get; }

        public string Password { get; }

        public List<UserDB> UserFriends { get; }

        // public List<Product> UserProducts { get; }

        public List<int> GroupMembership { get; }

        public UserCategory UserTraits { get; }
        
        public bool IsEnabled { get; } = true;

        public int? ImageId { get; }

        public int? Priviledge { get; }

        public DateTime CreatedOn { get; }

        public DateTime? LastEditedOn { get; }

        public UserDB(string login, string email, string password)
        {
            Login = login;
            Email = email;
            Password = password;
            CreatedOn = DateTime.Now;
        }
    }
}
