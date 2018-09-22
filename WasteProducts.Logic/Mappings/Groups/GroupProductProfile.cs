using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Mappings.Groups
{
    public class GroupProductProfile : Profile
    {
        public GroupProductProfile()
        {
            CreateMap<GroupProduct, GroupProductDB>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.GroupBoardId, y => y.MapFrom(z => z.GroupBoardId))
                .ForMember(x => x.Modified, y => y.Ignore())
                .ReverseMap();
        }
    }
}
