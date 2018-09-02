using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models
{
    public class ProductBoardComment
    {
        public virtual string Id { get; set; }

        public virtual UserDB Commentator { get; set; }

        public virtual string Comment { get; set; }
    }
}
