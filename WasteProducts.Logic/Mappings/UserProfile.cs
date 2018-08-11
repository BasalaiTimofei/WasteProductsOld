using System;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDB>()
                .ForMember(m => m.Created, opt => opt.MapFrom(u => u.Id == 0 ? DateTime.UtcNow : default(DateTime)))
                .ForMember(m => m.Modified, opt => opt.MapFrom(u => u.Id != default(int) ? DateTime.UtcNow : (DateTime?)null))
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.Ignore());
        }
    }
}
