using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.Logic.Common.Models;

namespace WasteProducts.Logic.Mappings
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDB>()
                .ForMember(x => x.TimeCreate, y => y.Ignore())
                .ForMember(x => x.Deleted, y => y.Ignore())
                .ForMember(x => x.IsDeleted, y => y.Ignore())
                .ForMember(x => x.GroupBoardDBs, y => y.MapFrom(z => z.GroupBoards))
                .ForMember(x => x.GroupUserDBs, y => y.MapFrom(z => z.GroupUsers))
                .ReverseMap();
        }
    }
}
