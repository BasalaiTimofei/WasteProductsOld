using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.UserMappings
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRole, UserRoleDB>();

            CreateMap<UserRoleDB, UserRole>();
        }
    }
}
