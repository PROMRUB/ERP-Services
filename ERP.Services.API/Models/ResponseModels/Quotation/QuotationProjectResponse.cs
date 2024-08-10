namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class QuotationProjectResponse
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    public int LeadTime { get; set; }
    public int Warranty { get; set; }
    public string PurchaseOrder { get; set; }
    public int Order { get; set; }
}