using AutoMapper;
using System.Globalization;
using WebApplication01.Data.Entities;
using WebApplication01.Models.Category;
using WebApplication01.Models.Product;
//фінальна версія маппера для проекту
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

        //маппінг для редагування продукту
        CreateMap<ProductEntity, ProductEditViewModel>()
            .ForMember(dest => dest.CategoryList, opt => opt.Ignore()) // Ігноруємо, оскільки заповнюємо вручну
            .ForMember(dest => dest.Photos, opt => opt.Ignore());       // Ігноруємо, бо це список файлів

        // Маппінг з ProductEditViewModel назад до ProductEntity
        CreateMap<ProductEditViewModel, ProductEntity>()
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
            .ForMember(x => x.Price, opt => opt.MapFrom(p => Decimal.Parse(p.Price.Replace('.', ','), new CultureInfo("uk-UA")))); ; // Ігноруємо ProductImages, бо додаємо їх окремо

    }
}
