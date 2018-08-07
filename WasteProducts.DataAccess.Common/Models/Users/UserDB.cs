using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    public class UserDB
    {
        public int UserId { get; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public DateTime CreatedOn { get; }
    }
}
