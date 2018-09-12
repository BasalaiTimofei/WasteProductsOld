using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDB>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Marked, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
