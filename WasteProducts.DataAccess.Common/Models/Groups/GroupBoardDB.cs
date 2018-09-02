using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models
{
    public class GroupBoardDB
    {
        /// <summary>
        /// Id - primary key
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Name - name board
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Information - additional information
        /// </summary>
        public virtual string Information { get; set; }

        /// <summary>
        /// GroupDB of this board
        /// </summary>
        public virtual GroupDB GroupDB { get; set; }

        /// <summary>
        /// UserId - user which created board
        /// </summary>
        public virtual UserDB Creator { get; set; }

        /// <summary>
        /// GroupProductDBs - products which add at board
        /// </summary>
        public virtual IList<GroupProductDB> GroupProductDBs { get; set; }

        /// <summary>
        /// true - group created
        /// false - group deleted
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// TimeCreate - datatime when group created
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Time of last modification.
        /// </summary>
        public virtual DateTime? Modified { get; set; }

        /// <summary>
        /// TimeDelete - datatime when group deleted
        /// </summary>
        public virtual DateTime? Deleted { get; set; }
    }
}
