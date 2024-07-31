using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Quotation;
using ERP.Services.API.Utils;

namespace ERP.Services.API.Interfaces;

public interface IQuotationService
{
    public Task<QuotationResponse> GetQuotationById(string keyword);
    public Task<QuotationResponse> Create(QuotationResource resource);
    public Task<QuotationResponse> Update(Guid id, QuotationResource resource);
    public Task<List<QuotationStatus>> QuotationStatus();
    public Task<QuotationResponse> UpdateStatus(Guid id, string status);
    public Task Delete(Guid id);
    public Task<QuotationResponse> Calculate(List<QuotationProductResource> resource);
    public Task<List<PaymentAccountResponse>> GetPaymentAccountListByBusiness(string orgId, Guid businessId);

    public Task<PaymentAccountResponse> GetPaymentAccountInformationById(string orgId, Guid businessId,
        Guid paymentAccountId);

    public Task<PagedList<QuotationResponse>> GetByList(string keyword, Guid businessId, int page, int pageSize);
    public Task<QuotationResponse> GetById(Guid id);
}