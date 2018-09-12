using AutoMapper;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Mappings.Products
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDB>()
                .ReverseMap();
        }
    }
}
