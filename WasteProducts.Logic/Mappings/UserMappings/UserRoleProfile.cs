using AutoMapper;
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
