using AutoMapper;
using System;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Barcods;
using System.Drawing;

namespace WasteProducts.Logic.Mappings.Barcods
{
    public class BarcodeProfile : Profile
    {
        public BarcodeProfile()
        {
            CreateMap<Barcode, BarcodeDB>()
                .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?)null))
                .ForMember(m => m.Product, opt => opt.Ignore())
                .ForMember(m => m.Created, opt => opt.Ignore());

            CreateMap<BarcodeDB, Barcode>()
                .ForMember(m => m.Brand, opt => opt.Ignore())
                .ForMember(m => m.Composition, opt => opt.Ignore())
                .ForMember(m => m.Country, opt => opt.Ignore())
                .ForMember(m => m.PicturePath, opt => opt.Ignore())
                .ForMember(m => m.Product, opt => opt.Ignore())
                .ForMember(m => m.ProductName, opt => opt.Ignore())
                .ForMember(m => m.Weight, opt => opt.Ignore());
        }
    }
}
