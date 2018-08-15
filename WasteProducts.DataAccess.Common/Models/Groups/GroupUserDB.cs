using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupUserDB
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public GroupDB Group { get; set; }
        public int UserId { get; set; }
        public List<GroupUserInviteTimeDB> GroupUserInviteTimeDBs { get; set; }
    }
}
