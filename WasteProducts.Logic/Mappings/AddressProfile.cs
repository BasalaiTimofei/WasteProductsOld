using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.DonationManagment;
using WasteProducts.Logic.Common.Models.DonationManagment;

namespace WasteProducts.Logic.Mappings
{
    class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDB>()
                .ForMember(m => m.CreatedOn, opt => opt.UseValue(DateTime.UtcNow))
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}