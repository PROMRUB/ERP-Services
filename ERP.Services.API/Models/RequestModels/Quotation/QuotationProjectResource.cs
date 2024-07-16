namespace ERP.Services.API.Models.RequestModels.Quotation;

public class QuotationProjectResource
{
    public Guid ProjectId { get; set; }
    public int LeadTime { get; set; }
    public int Warranty { get; set; }
    public string PaymentCondition { get; set; }
    public string PurchaseOrder { get; set; }
}