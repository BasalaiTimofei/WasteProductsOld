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
                .ForMember(x => x.TimeDelete, y => y.Ignore())
                .ForMember(x => x.Bool, y => y.Ignore());
        }
    }
}
