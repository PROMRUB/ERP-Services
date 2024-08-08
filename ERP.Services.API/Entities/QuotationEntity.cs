using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Entities;

[Table("Quotation")]
[Index(nameof(QuotationId), IsUnique = true)]
public class QuotationEntity
{
    [Key] [Column("quotation_id")] public Guid? QuotationId { get; set; }

    [Column("quotation_no")] public string? QuotationNo { get; set; }

    [Column("edit_time")] public int EditTime { get; set; }

    [Column("customer_id")] public Guid CustomerId { get; set; }

    public CustomerEntity Customer { get; set; }

    [Column("customer_contact_id")] public Guid CustomerContactId { get; set; }

    public CustomerContactEntity CustomerContact { get; set; }

    [Column("quotation_date")] public DateTime QuotationDateTime { get; set; }

    [Column("sale_person_id")] public Guid? SalePersonId { get; set; }

    public OrganizationUserEntity SalePerson { get; set; }

    [Column("issued_by")] public Guid? IssuedById { get; set; }

    public OrganizationUserEntity IssuedByUser { get; set; }


    public List<QuotationProductEntity> Products { get; set; }
    public List<QuotationProjectEntity> Projects { get; set; }

    [Column("amount")] public decimal Amount { get; set; }

    [Column("vat")] public decimal Vat { get; set; }

    [Column("price")] public decimal Price { get; set; }

    [Column("is_approved")] public bool IsApproved { get; set; }
    [Column("account_no")] public Guid? PaymentId { get; set; }
    public PaymentAccountEntity PaymentAccountEntity { get; set; }
    [Column("status")] public string Status { get; set; }
    [Column("business_id")] public Guid BusinessId { get; set; }
    [Column("remark")] public string Remark { get; set; }
    [Column("month")] public int Month { get; set; }
    [Column("year")] public int Year { get; set; }
    [Column("running")] public int Running { get; set; }


    public void SubmitStatus(string status)
    {
        EditTime += 1;
        QuotationDateTime = DateTime.UtcNow;
        Status = status;

        if (status == "อนุมัติ")
        {
            IsApproved = true;
        }
        else
        {
            IsApproved = false;
        }
    }


    public void Process()
    {
        Status = "Delete";
    }

    public void Update()
    {
        EditTime++;
    }
}