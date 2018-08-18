using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.UserMappings
{
    public class UserLoginProfile : Profile
    {
        public UserLoginProfile()
        {
            CreateMap<UserLogin, UserLoginInfo>().ConstructUsing((ul) => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey)).ReverseMap();
            CreateMap<UserLogin, IdentityUserLogin>().ForMember(m => m.UserId, opt => opt.Ignore()).ReverseMap();
        }
    }
}
