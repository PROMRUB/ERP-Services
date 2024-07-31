namespace ERP.Services.API.Models.RequestModels.Quotation;

public class QuotationProductResource
{
    public Guid QuotationId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int Discount { get; set; }
}