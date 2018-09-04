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
                .ForMember(x => x.Admin, y => y.Ignore())
                .ForMember(x => x.Modified, y => y.Ignore())
                .ForMember(x => x.IsDeleted, y => y.Ignore())
                .ForMember(x => x.GroupUsers, y => y.Ignore())
                .ReverseMap();
        }
    }
}
