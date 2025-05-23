﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("Product")]
    [Index(nameof(ProductId), nameof(OrgId), nameof(BusinessId), nameof(ProductCustomId), IsUnique = true)]
    public class ProductEntity
    {
        [Key] [Column("product_id")] public Guid? ProductId { get; set; }
        [Column("org_id")] public Guid? OrgId { get; set; }
        [Column("business_id")] public Guid? BusinessId { get; set; }
        [Column("product_cat_Id")] public Guid? ProductCatId { get; set; }
        [Column("product_sub_cat_Id")] public Guid? ProductSubCatId { get; set; }
        [Column("product_custom_Id")] public string? ProductCustomId { get; set; }
        [Column("product_name")] public string? ProductName { get; set; }
        [Column("msrp")] public decimal? MSRP { get; set; }
        [Column("lw_price")] public decimal? LwPrice { get; set; }
        [Column("currency_inhand")] public string? CurrencyInhand { get; set; }
        [Column("cost_inhand")] public decimal? CostInhand { get; set; }
        [Column("currency_last_po")] public string? CurrencyLastPO { get; set; }
        [Column("cost_last_po")] public decimal? CostLastPO { get; set; }
        [Column("product_status")] public string? ProductStatus { get; set; }
    }
}