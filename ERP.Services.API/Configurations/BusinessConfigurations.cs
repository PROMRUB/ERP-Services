using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Organization;
using ERP.Services.API.Models.ResponseModels.Organization;

namespace ERP.Services.API.Configurations
{
    public class BusinessConfigurations : Profile
    {
        public BusinessConfigurations()
        {
            CreateMap<OrganizationRequest, BusinessEntity>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.BusinessCreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.OrgName))
                .ForMember(dest => dest.BusinessCustomId, opt => opt.MapFrom(src => src.TaxId + "." + src.BrnId))
                .ForMember(dest => dest.BusinessDescription, opt => opt.MapFrom(src => src.OrgDescription))
                .ForMember(dest => dest.BusinessStatus, opt => opt.MapFrom(src => RecordStatus.Active));

            CreateMap<BusinessEntity, OrganizationResponse>()
                .ForMember(dest => dest.OrgName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.BrnId,
                    opt => opt.MapFrom(src => src.BrnId == "00000" ? "สำนักงานใหญ่ (00000)" : src.BrnId))
                .ForMember(dest => dest.OrgCustomId, opt => opt.MapFrom(src => src.BusinessCustomId));
        }
    }
}