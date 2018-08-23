using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.DonationManagment;
using WasteProducts.Logic.Common.Models.DonationManagment;

namespace WasteProducts.Logic.Mappings.DonationManagment
{
    class DonationProfile : Profile
    {
        public DonationProfile()
        {
            CreateMap<Donation, DonationDB>()
                .ForMember(m => m.CreatedOn, opt => opt.UseValue(DateTime.UtcNow))
                .ReverseMap();
        }
    }
}