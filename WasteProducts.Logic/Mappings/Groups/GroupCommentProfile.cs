using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Mappings.Groups
{
    public class GroupCommentProfile : Profile
    {
        public GroupCommentProfile()
        {
            CreateMap<GroupComment, GroupCommentDB>()
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.Parse(z.Id)))
                .ForMember(x => x.GroupBoardId, y => y.MapFrom(z => Guid.Parse(z.GroupBoardId)))
                .ForMember(x => x.Modified, y => y.Ignore())
                .ReverseMap();
        }
    }
}
