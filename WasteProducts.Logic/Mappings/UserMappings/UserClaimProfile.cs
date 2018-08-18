using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.UserMappings
{
    public class UserClaimProfile : Profile
    {
        public UserClaimProfile()
        {
            CreateMap<UserClaim, IdentityUserClaim>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.UserId, opt => opt.Ignore());

            CreateMap<IdentityUserClaim, UserClaim>();

            CreateMap<Claim, UserClaim>()
                .ForMember(m => m.ClaimType, opt => opt.MapFrom(m => m.Type))
                .ForMember(m => m.ClaimValue, opt => opt.MapFrom(m => m.Value));

            CreateMap<Claim, IdentityUserClaim>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.UserId, opt => opt.Ignore())
                .ForMember(m => m.ClaimType, opt => opt.MapFrom(m => m.Type))
                .ForMember(m => m.ClaimValue, opt => opt.MapFrom(m => m.Value));

            CreateMap<IdentityUserClaim, Claim>().ConvertUsing(s => new Claim(s.ClaimType, s.ClaimValue));
        }
    }
}
