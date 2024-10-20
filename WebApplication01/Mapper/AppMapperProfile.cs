using AutoMapper;
using System.Globalization;
using WebApplication01.Data.Entities;
using WebApplication01.Models.Category;
using WebApplication01.Models.Product;

namespace WebApplication01.Mapper;

public class AppMapperProfile : Profile
{
    public AppMapperProfile()
    {
        CreateMap<CategoryEntity, CategoryItemViewModel>();
        CreateMap<CategoryCreateViewModel, CategoryEntity>();

        CreateMap<ProductEntity, ProductItemViewModel>()
                .ForMember(x => x.Images, opt => opt.MapFrom(p => p.ProductImages.Select(x => x.Image).ToList()))
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(c => c.Category.Name));
        //string - . , - replace . (,)
        CreateMap<ProductCreateViewModel, ProductEntity>()
                 .ForMember(x => x.Price, opt => opt.MapFrom(p => Decimal.Parse(p.Price.Replace('.', ','), new CultureInfo("uk-UA"))));
    }
}
