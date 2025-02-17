﻿using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.ResponseModels.Customer;

namespace ERP.Services.API.Configurations
{
    public class CustomerConfigurations : Profile
    {
        public CustomerConfigurations()
        {
            CreateMap<CustomerRequest, CustomerEntity>()
                .ForMember(dest => dest.CusId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CusStatus, opt => opt.MapFrom(src => RecordStatus.Active.ToString()))
                .ForMember(dest => dest.CusCreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<CustomerEntity, CustomerResponse>()
                .ForMember(dest => dest.CusStatus, opt => opt.MapFrom(src => src.CusStatus == RecordStatus.Active.ToString() ? "รออนุมัติ" : "เลิกใช้งาน"));
            CreateMap<CustomerEntity, CustomerResponse>();

            CreateMap<CustomerContactRequest, CustomerContactEntity>()
                .ForMember(dest => dest.CusConId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CusConStatus, opt => opt.MapFrom(src => RecordStatus.Active.ToString()));
            CreateMap<CustomerContactEntity, CustomerContactResponse>()
                .ForMember(dest => dest.CusConName, opt => opt.MapFrom(src => src.CusConId + "." + src.CusConFirstname + " " + src.CusConLastname));
        }
    }
}
