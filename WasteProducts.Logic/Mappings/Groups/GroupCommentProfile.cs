using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Mappings.Groups
{
    public class GroupCommentProfile : Profile
    {
        public GroupCommentProfile()
        {
            CreateMap<GroupComment, GroupCommentDB>()
                .ForMember(x => x.Modified, y => y.Ignore())
                .ReverseMap();
        }
    }
}
