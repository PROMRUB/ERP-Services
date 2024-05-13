using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.ApiKey;
using ERP.Services.API.Models.ResponseModels.ApiKey;

namespace ERP.Services.API.Configurations
{
    public class ApiKeyConfigurations : Profile
    {
        public ApiKeyConfigurations()
        {
            CreateMap<ApiKeyEntity, ApiKeyResponse>();
            CreateMap<ApiKeyRequest, ApiKeyEntity>()
                .ForMember(dest => dest.KeyId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.KeyCreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
