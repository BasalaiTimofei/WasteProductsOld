using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    /// <summary>
    /// // TODO add XML documentation
    /// </summary>
    public class NewEmailConfirmator
    {
        public string UserId { get; set; }

        public string NewEmail { get; set; }
        
        public string Token { get; set; }

        public DateTime Created { get; set; }
    }
}
