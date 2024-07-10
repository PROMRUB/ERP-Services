using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Condition;
using ERP.Services.API.Models.ResponseModels.Condition;

namespace ERP.Services.API.Configurations
{
    public class ConditionConfigurations : Profile
    {
        public ConditionConfigurations() {
            CreateMap<ConditionRequest, ConditionEntity>()
                .ForMember(dest => dest.ConditionId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.ConditionStatus, opt => opt.MapFrom(_ => RecordStatus.Active.ToString()));

            CreateMap<ConditionEntity, ConditionResponse>();
        }
    }
}
