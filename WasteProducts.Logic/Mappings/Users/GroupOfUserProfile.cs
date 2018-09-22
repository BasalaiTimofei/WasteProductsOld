using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.Users
{
    public class GroupOfUserProfile : Profile
    {
        public GroupOfUserProfile()
        {
            CreateMap<GroupUserDB, GroupOfUser>().ForMember(m => m.Name, cfg => cfg.MapFrom(g => g.Group.Name));
        }
    }
}
