using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    public class UserLoginDb : IUserLoginDb
    {
        private IUserDb _user;

        #region Scalar Properties
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual int UserId { get; set; }
        #endregion

        #region Navigation Properties
        public virtual IUserDb User
        {
            get { return _user; }
            set
            {
                _user = value;
                UserId = value.Id;
            }
        }
        #endregion
    }
}
