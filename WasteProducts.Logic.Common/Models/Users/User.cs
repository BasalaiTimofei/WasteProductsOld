using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models.Users
{
    public class User
    {
        public int Id { get; }

        public string Login { get; }

        public string Email { get; }

        public string Password { get; }

        public List<User> UserFriends { get; }

        // public List<Product> UserProducts { get; }

        public List<int> GroupMembership { get; }

        // [TBD] public UserCategory UserTraits { get; }

        public bool IsEnabled { get; } = true;

        public int? ImageId { get; }

        public int? Priviledge { get; }

        public User(string login, string email, string password)
        {
            Login = login;
            Email = email;
            Password = password;
        }
    }
}
