using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.UserMappings
{
    public class UserLoginProfile : Profile
    {
        public UserLoginProfile()
        {
            CreateMap<UserLogin, UserLoginDB>().ReverseMap();
            CreateMap<UserLogin, IdentityUserLogin>().ForMember(m => m.UserId, opt => opt.Ignore());
        }
    }
}
