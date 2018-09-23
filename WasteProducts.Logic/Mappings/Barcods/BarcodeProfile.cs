using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Mappings.Barcods
{
    public class BarcodeProfile : Profile
    {
        public BarcodeProfile()
        {
            CreateMap<Barcode, BarcodeDB>()
                .ForMember(m => m.Created,
                    opt => opt.MapFrom(p => p.ProductName != null ? DateTime.UtcNow : default(DateTime)))
                .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?)null))
                .ReverseMap();
        }
    }
}
