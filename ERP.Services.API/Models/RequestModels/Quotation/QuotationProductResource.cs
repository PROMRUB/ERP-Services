namespace ERP.Services.API.Models.RequestModels.Quotation;

public class QuotationProductResource
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public float Discount { get; set; }
    public float Amount { get; set; }
}