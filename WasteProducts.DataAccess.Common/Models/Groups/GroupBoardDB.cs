using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupBoardDB
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public int GroupId { get; set; }
        public GroupDB GroupDB { get; set; }
        public int UserId { get; set; }
        public List<ProductBoardDB> ProductBoardDBs { get; set; }
        public DateTime Create { get; set; }
        public DateTime Delete { get; set; }
    }
}
