using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models
{
    public class GroupUserDB
    {
        /// <summary>
        /// Id - secondary key
        /// </summary>
        public virtual GroupDB GroupDB { get; set; }

        /// <summary>
        /// UserId - user which entered in group
        /// </summary>
        public virtual UserDB User { get; set; }

        //TODO заполнить
        /// <summary>
        /// 
        /// </summary>
        public virtual bool RigtToCreateBoards { get; set; }

        /// <summary>
        /// Invite - user action wShen administrator inviting
        ///     0 - invite send;
        ///     1 - invite user confirmed;
        ///     2 - invite user rejected;
        /// </summary>
        public virtual int? Bool { get; set; }
    }
}
