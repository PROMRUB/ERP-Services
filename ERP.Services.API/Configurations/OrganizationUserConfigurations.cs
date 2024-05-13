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
            CreateMap<OrganizationUserEntity, OrganizationUserResponse>();
            CreateMap<OrganizationUserRequest, OrganizationUserEntity>();
        }
    }
}
