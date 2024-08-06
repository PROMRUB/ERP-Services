using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Entities;

[Table("QuotationProduct")]
[Index(nameof(QuotationId), nameof(ProductId), IsUnique = true)]
public class QuotationProductEntity
{
    [Column("quotation_id")] public Guid QuotationId { get; set; }
    public QuotationEntity Quotation { get; set; }
    [Column("product_id")] public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; }

    [Column("quantity")] public int Quantity { get; set; }

    [Column("price")] public int Price { get; set; }
    [Column("order")] public int Order { get; set; }
    [Column("amount")] public float Amount { get; set; }
    [Column("discount")] public float Discount { get; set; }
}