using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Models.Security.Models
{
    /// <summary>
    /// Class ClaimDb. Has an inheritance from IRoleDb. Security model class
    /// </summary>
    public class RoleDb : IRoleDb
    {
        #region Fields
        /// <summary>
        /// user field collection
        /// </summary>
        private ICollection<IUserDb> _users;
        #endregion

        #region Scalar Properties
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// Navigation user collection property 
        /// </summary>
        public ICollection<IUserDb> Users
        {
            get { return _users ?? (_users = new List<IUserDb>()); }
            set { _users = value; }
        }
        #endregion
    }
}
