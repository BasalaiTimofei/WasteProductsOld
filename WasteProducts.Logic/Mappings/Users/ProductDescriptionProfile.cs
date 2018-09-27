using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.Users
{
    public class UserProductProfile : Profile
    {
        public UserProductProfile()
        {
            CreateMap<UserProductDescriptionDB, UserProduct>();
        }
    }
}
