namespace ERP.Services.API.Models.RequestModels.Quotation;

public class QuotationProjectResource
{
    public Guid ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public int LeadTime { get; set; }
    public int Warranty { get; set; }
    public Guid ConditionId { get; set; }
    public string PurchaseOrder { get; set; }
    public string? EthSaleMonth { get; set; }
}