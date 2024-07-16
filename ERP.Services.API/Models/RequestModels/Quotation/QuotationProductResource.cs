namespace ERP.Services.API.Models.RequestModels.Quotation;

public class QuotationProductResource
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; }
    public double Amount { get; set; }
    public double BasePrice { get; set; }
}