using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    public class UserRoleDb : IUserRoleDb
    {
        private IUserDb _user;
        private IRoleDb _role;

        #region Scalar Properties
        public virtual int UserId { get; set; }
        public virtual int RoleId { get; set; }
        #endregion

        #region Navigation Properties
        public virtual IRoleDb Role
        {
            get { return _role; }
            set
            {
                _role = value;
                RoleId = value.RoleId;
            }
        }

        public virtual IUserDb User
        {
            get { return _user; }
            set
            {
                _user = value;
                UserId = value.UserId;
            }
        }
        #endregion

    }

}
