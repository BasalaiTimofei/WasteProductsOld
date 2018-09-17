using System;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Barcodes;
using WasteProducts.Logic.Common.Models.Barcodes;

namespace WasteProducts.Logic.Mappings.Barcodes
{
    public class BarcodeProfile : Profile
    {
        public BarcodeProfile()
        {
            CreateMap<Barcode, BarcodeDB>()
                .ForMember(m => m.Created,
                    opt => opt.MapFrom(b => b.ProductName != null ? DateTime.UtcNow : default(DateTime)))
                .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?)null))
                .ReverseMap();
        }
    }
}
