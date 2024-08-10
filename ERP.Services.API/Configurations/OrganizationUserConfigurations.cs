using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.Organization;
using ERP.Services.API.Models.ResponseModels.Organization;

namespace ERP.Services.API.Configurations
{
    public class OrganizationUserConfigurations : Profile
    {
        public OrganizationUserConfigurations()
        {
            CreateMap<OrganizationUserRequest, OrganizationUserEntity>()
                .ForMember(dest => dest.OrgUserId, opt => opt.MapFrom(_ => Guid.NewGuid()));
            CreateMap<OrganizationUserEntity, OrganizationUserResponse>()
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(x => x.FirstNameTh))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(x => x.LastnameTh))
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(x => x.FirstNameTh + " " + x.LastnameTh))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => new List<string>()));
        }
    }
}
