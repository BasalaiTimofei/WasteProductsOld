using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Groups;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Mappings.Groups
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDB>()
                .ForMember(x => x.Created, y => y.Ignore())
                .ForMember(x => x.Deleted, y => y.Ignore())
                .ForMember(x => x.Modified, y => y.Ignore())
                .ForMember(x => x.IsNotDeleted, y => y.Ignore())
                .ReverseMap();
        }
    }
}
