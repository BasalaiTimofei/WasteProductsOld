using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.Logic.Common.Models.Donations;

namespace WasteProducts.Logic.Mappings.Donations
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDB>()
                .ForMember(m => m.Created, opt => opt.Ignore())
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Donors, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}