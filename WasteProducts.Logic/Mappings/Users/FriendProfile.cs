using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.Users
{
    class FriendProfile : Profile
    {
        public FriendProfile()
        {
            CreateMap<UserDAL, Friend>();
        }
    }
}
