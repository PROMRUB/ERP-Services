using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Entities
{
    [Table("CustomerNo")]
    [Index(nameof(CusNoId), IsUnique = true)]
    [Index(nameof(OrgId), nameof(BusinessId), nameof(Character), IsUnique = true)]
    public class CustomerNumberEntity
    {
        [Key]
        [Column("cus_no_id")]
        public Guid? CusNoId { get; set; }

        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("business_id")]
        public Guid? BusinessId { get; set; }

        [Column("character")]
        public string? Character { get; set; }

        [Column("allocated")]
        public int? Allocated { get; set; }
    }
}
