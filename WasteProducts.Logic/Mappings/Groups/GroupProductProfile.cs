using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.Logic.Common.Models;

namespace WasteProducts.Logic.Mappings
{
    public class GroupProductProfile : Profile
    {
        public GroupProductProfile()
        {
            CreateMap<GroupProduct, GroupProductDB>();
        }
    }
}
