using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class ProductBoardDB
    {
        public int Id { get; set; }
        public int GroupBordId { get; set; }
        public GroupBoardDB GroupBoardDB { get; set; }
        public int ProductId { get; set; }
        public string Information { get; set; }
    }
}
