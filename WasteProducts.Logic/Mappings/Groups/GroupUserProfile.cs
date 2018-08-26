using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.Logic.Common.Models;

namespace WasteProducts.Logic.Mappings
{
    public class GroupUserProfile : Profile
    {
        public GroupUserProfile()
        {
            CreateMap<GroupUser, GroupUserDB>()
                .ForMember(x => x.GroupDB, y => y.Ignore())
                .ForMember(x => x.Bool, y => y.Ignore())
                .ForMember(x => x.GroupUserInviteTimeDBs, y => y.Ignore());
        }
    }
}
