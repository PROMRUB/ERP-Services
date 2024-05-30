using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.ApiKey;
using ERP.Services.API.Models.ResponseModels.ApiKey;
using ERP.Services.API.Models.ResponseModels.SystemConfig;

namespace ERP.Services.API.Configurations
{
    public class SystemConfigConfigurations : Profile
    {
        public SystemConfigConfigurations()
        {
            CreateMap<ProvinceEntity, ProvinceResponse>()
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.ProvinceNameTh));
            CreateMap<DistrictEntity, DistrictResponse>()
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.DistrictNameTh));
            CreateMap<SubDistrictEntity, SubDIstrictResponse>()
                .ForMember(dest => dest.SubDistrictName, opt => opt.MapFrom(src => src.SubDistrictNameTh));
        }
    }
}
