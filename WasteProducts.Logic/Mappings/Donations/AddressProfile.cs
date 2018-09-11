using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.Logic.Common.Models.Donations;

namespace WasteProducts.Logic.Mappings.Donations
{
    class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDB>()
                .ForMember(m => m.CreatedOn, opt => opt.Ignore())
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Donors, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}