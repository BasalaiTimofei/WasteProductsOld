using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Enums;

namespace WasteProducts.DataAccess.Common.Models.Users
{
    public class UserDB
    {
        /// <summary>
        /// Unique identifier of concrete User in Database.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Unique name of concrete User. It is used for authenfication.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Email of User is mandatory property and it is set during registration. It is used for password recovery.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password is set by User during registration.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// List of Users which belong to group of friends related to current User.
        /// </summary>
        public List<UserDB> UserFriends { get; set; }

        /// <summary>
        /// List of Products which User have ever captured.
        /// </summary>
        //public List<Product> UserProducts { get; set; }

        /// <summary>
        /// List of all Groups to which current User is assigned.
        /// </summary>
        //public List<Group> GroupMembership { get; set; }

        /// <summary>
        /// Defines User's belonging to some specific Group of customers. E.g. User can be diabetics and prefere low carb Products. Or sportsmen and prefere high protein diet.
        /// </summary>
        public int UserTraits { get; set; }
        
        /// <summary>
        /// Defines if User is banned by Admin or not. True refers to not banned. False refers to banned.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Identifier of image that is avatar of User. Can be NULL - without avatar.
        /// </summary>
        public int? ImageId { get; set; }

        /// <summary>
        /// Privildge which assigned to the User. Can be User or Admin.
        /// </summary>
        public int Priviledge { get; set; }

        /// <summary>
        /// Specifies timestamp of creation of concrete User in Database.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Specifies timestamp of modifying of any Property of User in Database.
        /// </summary>
        public DateTime? LastEditedOn { get; set; }
    }
}
