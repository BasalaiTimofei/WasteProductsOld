using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Mappings.Groups
{
    public class GroupBoardProfile : Profile
    {
        public GroupBoardProfile()
        {
            CreateMap<GroupBoard, GroupBoardDB>()
                .ForMember(x => x.Created, y => y.Ignore())
                .ForMember(x => x.Deleted, y => y.Ignore())
                .ForMember(x => x.Modified, y => y.Ignore())
                .ForMember(x => x.IsDeleted, y => y.Ignore())
                .ReverseMap();
        }
    }
}
