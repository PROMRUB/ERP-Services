using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Project;
using ERP.Services.API.Models.ResponseModels.Project;

namespace ERP.Services.API.Configurations
{
    public class ProjectConfigurations : Profile
    {
        public ProjectConfigurations() {
            CreateMap<ProjectRequest, ProjectEntity>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(x => Guid.NewGuid()))
                .ForMember(dest => dest.ProjectStatus, opt => opt.MapFrom(x => RecordStatus.Active.ToString()));
            CreateMap<ProjectEntity, ProjectResponse>();
        }
    }
}
