using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    public class RoleDb : IRoleDb
    {
        #region Fields
        private ICollection<IUserDb> _users;
        #endregion

        #region Scalar Properties
        public int Id { get; set; }
        public string Name { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<IUserDb> Users
        {
            get { return _users ?? (_users = new List<IUserDb>()); }
            set { _users = value; }
        }
        #endregion
    }
}
