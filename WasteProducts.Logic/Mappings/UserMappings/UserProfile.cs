using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
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
                .ForMember(m => m.Created, opt => opt.Ignore())
                .ForMember(m => m.Modified, opt => opt.Ignore())
                .ForMember(m => m.Roles, opt => opt.Ignore())
                .ForMember(m => m.PasswordHash, opt => opt.MapFrom(u => u.Password));

            CreateMap<UserDB, User>()
                .ForMember(m => m.Roles, opt => opt.ResolveUsing((u, u2) => new List<string>()))
                .ForMember(m => m.Password, opt => opt.MapFrom(u => u.PasswordHash));
        }
    }
}
