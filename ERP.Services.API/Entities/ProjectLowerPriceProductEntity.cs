// using System.ComponentModel.DataAnnotations.Schema;
//
// namespace ERP.Services.API.Entities;
//
// [Table("ProjectLowerPriceProduct")]
// public class ProjectLowerPriceProductEntity
// {
//     [Column("project_id")]
//     public Guid ProjectId { get; set; }
//
//     public ProjectEntity Project { get; set; }
//
//     [Column("product_id")]
//     public Guid ProductId { get; set; }
//
//     public ProductEntity Product { get; set; }
//
//     [Column("customer_id")]
//     public Guid CustomerId { get; set; }
//     public CustomerEntity Customer { get; set; }
//
//     [Column("price")]
//     public decimal Price { get; set; }
//
//     [Column("quotation_reference")]
//     public string? Quotationreference { get; set; }
//
// }