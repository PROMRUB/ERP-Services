namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class QuotationListResponse
{
    public Guid QuotationId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public Guid ProjectId { get; set; }
    public decimal Amount { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public string SalMonth { get; set; }
    public string Status { get; set; }
}