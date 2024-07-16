namespace ERP.Services.API.Models.RequestModels.Quotation;

public class QuotationResource
{
    public Guid? CustomerId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? ContactPersonId { get; set; }
    public Guid? SalePersonId { get; set; }
    public Guid? IssuedById { get; set; }

    public List<QuotationProductResource> Products { get; set; }
    public List<QuotationProjectResource> Projects { get; set; }
}