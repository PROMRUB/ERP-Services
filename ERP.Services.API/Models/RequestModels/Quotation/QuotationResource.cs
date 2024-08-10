namespace ERP.Services.API.Models.RequestModels.Quotation;


public class QuotationResource
{
    public Guid QuotationId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ContactPersonId { get; set; }
    public Guid? SalesPersonId { get; set; }
    public Guid? IssuedById { get; set; }
    public Guid BusinessId { get; set; }

    public List<QuotationProductResource> Products { get; set; }
    public List<QuotationProjectResource> Projects { get; set; }
    public string Remark { get; set; }
    public string Status { get; set; } = "รออนุมัติ";
    public Guid? PaymentAccountId { get; set; }
}