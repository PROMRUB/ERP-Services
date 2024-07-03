using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("ProductCategory")]
    [Index(nameof(CustomCatId), IsUnique = true)]
    public class ProductCategoryEntity
    {
        [Key]
        [Column("cat_id")]
        public Guid ProductCatId { get; set; }
        [Column("org_id")]
        public Guid? OrgId { get; set; }
        [Column("business_id")]
        public Guid? BusinessId { get; set; }
        [Column("cat_cus_id")]
        public string? CustomCatId { get; set; }
        [Column("parent_cat_id")]
        public string? ParentCatId { get; set; }
        [Column("cat_name")]
        public string? CategoryName { get; set; }
        [Column("cat_status")]
        public string? CategoryStatus { get; set; }
    }
}
