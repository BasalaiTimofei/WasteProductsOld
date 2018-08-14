using System;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.UserMappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDB>()
                .ForMember(m => m.Created, opt => opt.Ignore())
                .ForMember(m => m.Modified, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
