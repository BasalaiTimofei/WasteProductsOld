using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Mappings.Groups
{
    public class GroupUserProfile : Profile
    {
        public GroupUserProfile()
        {
            CreateMap<GroupUser, GroupUserDB>()
                .ReverseMap();
        }
    }
}
