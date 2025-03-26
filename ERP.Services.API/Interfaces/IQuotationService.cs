using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Quotation;
using ERP.Services.API.Utils;
using static ERP.Services.API.Controllers.v1.QuotationController;

namespace ERP.Services.API.Interfaces;

public interface IQuotationService
{
    public Task<QuotationResource> GetQuotationById(string keyword);
    public Task<QuotationResource> Create(QuotationResource resource);
    public Task<QuotationResource> Update(Guid id, QuotationResource resource);
    public Task<List<QuotationStatus>> QuotationStatus();
    public Task<QuotationResource> UpdateStatus(Guid id, QuotationResource status);
    public Task Delete(Guid id);
    public Task<QuotationResponse> Calculate(List<QuotationProductResource> resource);
    public Task<List<PaymentAccountResponse>> GetPaymentAccountListByBusiness(string orgId, Guid businessId);

    public Task<PaymentAccountResponse> GetPaymentAccountInformationById(string orgId, Guid businessId,
        Guid paymentAccountId);

    public Task<PurchaseDetail> GetProductPurchaseDetail(Guid quotationId, Guid productId);

    public Task<PagedList<QuotationResponse>> GetByList(string keyword, Guid businessId, string? startDate,
        string? endDate, Guid? customerId, Guid? projectId, int? profit, bool? isSpecialPrice, Guid? salePersonId,
        string? status, int page, int pageSize, bool? isGreaterThan);

    public Task<TotalProductQuotation> GetTotalProductQuotation(Guid id);
    public Task<QuotationResource> UpdateCostEstimateQuotation(Guid id, Guid productId, UpdateProductQuotationParameter request);
    public Task<QuotationResource> GetById(Guid id);
    public Task<QuotationResource> ApproveSalePrice(Guid id);
    public Task<QuotationResource> ApproveQuotation(Guid id);
    public Task<QuotationDocument> GeneratePDF(Guid id);
    public Task SendGeneralMail(string to, string toName, string subject, string body);
    public Task DeleteAll();
}