using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.Logic.Common.Models.Donations;

namespace WasteProducts.Logic.Mappings.Donations
{
    class DonorProfile : Profile
    {
        public DonorProfile()
        {
            CreateMap<Donor, DonorDB>()
                .ForMember(m => m.CreatedOn, opt => opt.Ignore())
                .ForMember(m => m.ModifiedOn, opt => opt.Ignore())
                .ForMember(m => m.Donations, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}