using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.PaymentAccount;
using ERP.Services.API.Models.RequestModels.Project;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Project;
using ERP.Services.API.Repositories;
using ERP.Services.API.Utils;

namespace ERP.Services.API.Interfaces
{
    public interface IPaymentAccountService
    {
        public Task<List<PaymentAccountResponse>> GetPaymentAccountListByBusiness(string orgId, Guid businessId);
        public Task<PagedList<PaymentAccountResponse>> GetPaymentAccountListByBusiness(string orgId, Guid businessId,string keyword,int page,int pageSize);
        public Task<PaymentAccountResponse> GetPaymentAccountInformationById(string orgId, Guid businessId, Guid projectId);
        public Task CreatePaymentAccount(string orgId, PaymentAccountRequest request);
        public Task UpdatePaymentAccount(string orgId, Guid businessId, Guid paymentAccountId, PaymentAccountRequest request);
        public Task DeletePaymentAccount(string orgId, Guid businessId, Guid paymentAccountId, PaymentAccountRequest request);
    }
}
