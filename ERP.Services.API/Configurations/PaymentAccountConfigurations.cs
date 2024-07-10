using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;

namespace ERP.Services.API.Configurations
{
    public class PaymentAccountConfigurations : Profile
    {
        PaymentAccountConfigurations()
        {
            CreateMap<PaymentAccountRequest, PaymentAccountEntity>()
                    .ForMember(dest => dest.PaymentAccountId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                    .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(_ => RecordStatus.Active.ToString()));

            CreateMap<PaymentAccountEntity, PaymentAccountResponse>();
        }
    }
}
