using AutoMapper;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.UserMappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDB>()
                .ForMember(m => m.Roles, opt => opt.Ignore());

            CreateMap<UserDB, User>()
                .ForMember(m => m.Roles, opt => opt.ResolveUsing((u, u2) => new List<string>()));
        }
    }
}
