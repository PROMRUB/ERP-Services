using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Models.ResponseModels.Role;

namespace ERP.Services.API.Configurations
{
    public class RoleConfigurations : Profile
    {
        public RoleConfigurations()
        {
            CreateMap<RoleEntity, RoleListResponse>();
        }
    }
}
