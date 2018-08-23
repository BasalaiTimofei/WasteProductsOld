using System;
using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDB>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Created, opt => opt.MapFrom(p => p.Name != null ? DateTime.UtcNow : default(DateTime)))
                .ForMember(m => m.Modified, opt => opt.UseValue((DateTime?)null))
                .ReverseMap()
                .ForMember(m => m.Id, opt => opt.MapFrom(p => p.Id.ToString()));
        }
    }
}
