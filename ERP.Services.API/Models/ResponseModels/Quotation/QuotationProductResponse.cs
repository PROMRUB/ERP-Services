namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class QuotationProductResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; }
    public decimal Amount { get; set; }
    public decimal BasePrice { get; set; }
    public int Order { get; set; }
}