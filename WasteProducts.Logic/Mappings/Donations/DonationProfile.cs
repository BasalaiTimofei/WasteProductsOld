using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.Logic.Common.Models.Donations;

namespace WasteProducts.Logic.Mappings.Donations
{
    class DonationProfile : Profile
    {
        public DonationProfile()
        {
            CreateMap<Donation, DonationDB>()
                .ForMember(m => m.Created, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}