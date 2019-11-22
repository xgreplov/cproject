using AutoMapper;
using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.DataTransferObjects.Common;
using DemoEshop.BusinessLayer.DataTransferObjects.Filters;
using DemoEshop.DataAccessLayer.EntityFramework.Entities;
using DemoEshop.Infrastructure.Query;

namespace DemoEshop.BusinessLayer.Config
{
    public class MappingConfig
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<Product, ProductDto>();
            config.CreateMap<ProductDto, Product>().ForMember(dest => dest.Category, opt => opt.Ignore());
            config.CreateMap<Category, CategoryDto>().ForMember(categoryDto => categoryDto.CategoryPath, opts => opts.ResolveUsing(category =>
                {
                    var categoryPath = category.Name;
                    while (category.Parent != null)
                    {
                        categoryPath = category.Parent.Name + "/" + categoryPath;
                        category = category.Parent;
                    }
                    return categoryPath;
                })).ReverseMap();
            config.CreateMap<Rating, RatingDto>().ReverseMap();
            config.CreateMap<RateSong, RateSongDto>().ReverseMap();
            config.CreateMap<Customer, CustomerDto>().ReverseMap();
            config.CreateMap<User, UserDto>().ReverseMap();
            config.CreateMap<User, UserCreateDto>().ReverseMap();
            config.CreateMap<Customer, UserCreateDto>().ReverseMap();
            config.CreateMap<QueryResult<Product>, QueryResultDto <ProductDto, ProductFilterDto>>();
            config.CreateMap<QueryResult<Category>, QueryResultDto<CategoryDto, CategoryFilterDto>>();
            config.CreateMap<QueryResult<Rating>, QueryResultDto<RatingDto, RatingFilterDto>>();
            config.CreateMap<QueryResult<RateSong>, QueryResultDto<RateSongDto, RateSongFilterDto>>();
            config.CreateMap<QueryResult<Customer>, QueryResultDto<CustomerDto, CustomerFilterDto>>();
            config.CreateMap<QueryResult<User>, QueryResultDto<UserDto, UserFilterDto>>();
        }

    }
}
