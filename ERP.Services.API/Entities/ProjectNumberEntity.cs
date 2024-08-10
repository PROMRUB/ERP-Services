using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("ProjectNo")]
    [Index(nameof(CusNoId), IsUnique = true)]
    [Index(nameof(OrgId), nameof(BusinessId), nameof(Year), IsUnique = true)]
    public class ProjectNumberEntity
    {
        [Key]
        [Column("cus_no_id")]
        public Guid? CusNoId { get; set; }

        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("business_id")]
        public Guid? BusinessId { get; set; }

        [Column("year")]
        public string? Year { get; set; }

        [Column("allocated")]
        public int? Allocated { get; set; }
    }
}
