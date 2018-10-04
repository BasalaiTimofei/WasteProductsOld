using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Users;

namespace WasteProducts.Logic.Mappings.Users
{
    public class GroupOfUserProfile : Profile
    {
        public GroupOfUserProfile()
        {
            CreateMap<GroupUserDB, GroupOfUser>()
                .ForMember(m => m.Id, cfg => cfg.MapFrom(g => g.Group.Id))
                .ForMember(m => m.AdminId, cfg => cfg.MapFrom(g => g.Group.AdminId))
                .ForMember(m => m.Name, cfg => cfg.MapFrom(g => g.Group.Name))
                .ForMember(m => m.Information, cfg => cfg.MapFrom(g => g.Group.Information));
        }
    }
}
