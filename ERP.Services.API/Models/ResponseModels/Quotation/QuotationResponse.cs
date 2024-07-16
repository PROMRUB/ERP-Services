namespace ERP.Services.API.Models.ResponseModels.Quotation;

public class QuotationResponse
{
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
    public string Price { get; set; }
    public string Vat { get; set; }
    public string Amount { get; set; }
    public string AccountNo { get; set; }
        
}