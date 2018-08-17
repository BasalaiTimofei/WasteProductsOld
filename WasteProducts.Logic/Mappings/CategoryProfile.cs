using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Mappings
{
    class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDB>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
