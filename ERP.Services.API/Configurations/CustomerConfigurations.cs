using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.ResponseModels.Customer;

namespace ERP.Services.API.Configurations
{
    public class CustomerConfigurations : Profile
    { 
        public CustomerConfigurations() {
            CreateMap<CustomerRequest, CustomerEntity>()
                .ForMember(dest => dest.CusStatus, opt => opt.MapFrom(src => RecordStatus.Active))
                .ForMember(dest => dest.CusCreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<CustomerEntity, CustomerResponse>();
        }
    }
}
