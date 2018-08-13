using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupDB
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public int Admin { get; set; }
        public List<GroupUserDB> GroupUsers { get; set; }
        public List<GroupBordDB> GroupBords { get; set; }
        public DateTime Create { get; set; }
        public DateTime Delete { get; set; }
    }
}