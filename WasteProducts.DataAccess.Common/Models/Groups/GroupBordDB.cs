using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    class GroupBordDB
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public int GroupId { get; set; }
        public GroupDB Group { get; set; }
        public int UserId { get; set; }
        public List<ProductBordDB> ProductBords { get; set; }
        public DateTime Create { get; set; }
        public DateTime Delete { get; set; }
    }
}
