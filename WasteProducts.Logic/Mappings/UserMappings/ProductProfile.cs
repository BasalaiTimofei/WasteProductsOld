using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Barcods;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Mappings.UserMappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDB>()
                .ForMember(m => m.Created, opt => opt.Ignore())
                .ForMember(m => m.Modified, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Barcode, BarcodeDB>()
                .ForMember(m => m.Created, opt => opt.Ignore())
                .ForMember(m => m.Modified, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Category, CategoryDB>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Marked, opt => opt.Ignore())
                .ForMember(m => m.Products, opt => opt.Ignore())
                .ReverseMap();
        }
        
    }
}
