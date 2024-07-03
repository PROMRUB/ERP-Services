using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.ResponseModels.Product;

namespace ERP.Services.API.Configurations
{
    public class ProductConfigurations : Profile
    {
        public ProductConfigurations() {
            CreateMap<ProductRequest, ProductEntity>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(x => Guid.NewGuid()))
                .ForMember(dest => dest.ProductStatus, opt => opt.MapFrom(x => RecordStatus.Active.ToString()));
            CreateMap<ProductEntity, ProductResponse>();

            CreateMap<ProductCategoryRequest, ProductCategoryEntity>()
                .ForMember(dest => dest.ProductCatId, opt => opt.MapFrom(x => Guid.NewGuid()))
                .ForMember(dest => dest.CategoryStatus, opt => opt.MapFrom(x => RecordStatus.Active.ToString()));


            CreateMap<ProductEntity, ProductResponse>();
            CreateMap<ProductCategoryEntity, ProductCategoryResponse>();
        }
    }
}
