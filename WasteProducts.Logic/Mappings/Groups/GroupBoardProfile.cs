using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Mappings.Groups
{
    public class GroupBoardProfile : Profile
    {
        public GroupBoardProfile()
        {
            CreateMap<GroupBoard, GroupBoardDB>()
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.Parse(z.Id)))
                .ForMember(x => x.GroupId, y => y.MapFrom(z => Guid.Parse(z.GroupId)))
                .ForMember(x => x.Created, y => y.Ignore())
                .ForMember(x => x.Deleted, y => y.Ignore())
                .ForMember(x => x.Modified, y => y.Ignore())
                .ForMember(x => x.IsNotDeleted, y => y.Ignore())
                .ReverseMap();
        }
    }
}
