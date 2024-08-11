namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class QuotationResponse
{
    public Guid QuotationId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerNo { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public string ContactPerson { get; set; }
    public Guid ContactPersonId { get; set; }
    public string QuotationNo { get; set; }
    public string QuotationDateTime { get; set; }
    public int EditTime { get; set; }
    public Guid? IssuedByUser { get; set; }
    public string IssuedByUserName { get; set; }
    public Guid? SalePersonId { get; set; }
    public string SalePersonIName{ get; set; }
    public string Status { get; set; }
    public List<QuotationProductResponse> Products { get; set; }
    public List<QuotationProjectResponse> Projects { get; set; }
    public decimal Price { get; set; }
    public decimal Vat { get; set; }
    public decimal Amount { get; set; }
    public Guid AccountNo { get; set; }
    public string Remark { get; set; }
    public Guid? IssuedByUserId { get; set; }

    public decimal RealPriceMsrp { get; set; }
    public decimal SumOfDiscount { get; set; }
    public decimal AmountBeforeVat { get; set; }
}