using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Organization;
using ERP.Services.API.Models.ResponseModels.Organization;

namespace ERP.Services.API.Configurations
{
    public class OrganizationConfigurations : Profile
    {
        public OrganizationConfigurations()
        {
            CreateMap<OrganizationRequest, OrganizationEntity>()
                .ForMember(dest => dest.OrgId, opt => opt.MapFrom(x => Guid.NewGuid()))
                .ForMember(dest => dest.OrgCreatedDate, opt => opt.MapFrom(x => DateTime.UtcNow))
                .ForMember(dest => dest.OrgStatus, opt => opt.MapFrom(x => RecordStatus.Active));
            CreateMap<OrganizationEntity, OrganizationResponse>()
                .ForMember(dest => dest.OrgName, opt => opt.MapFrom(src => src.DisplayName));
        }
    }
}
