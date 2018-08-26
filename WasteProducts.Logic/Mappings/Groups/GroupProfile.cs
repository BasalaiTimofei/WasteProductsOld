using AutoMapper;
using System.Text.RegularExpressions;
using WasteProducts.DataAccess.Common.Models;

namespace WasteProducts.Logic.Mappings
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDB>();
        }
    }
}
