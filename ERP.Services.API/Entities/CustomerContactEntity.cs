using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("CustomerContact")]
    [Index(nameof(CusConId), IsUnique = true)]
    public class CustomerContactEntity
    {
        public CustomerContactEntity()
        {
            this.CusConId = Guid.NewGuid();
        }

        [Key]
        [Column("cus_con_id")]
        public Guid? CusConId { get; set; }
        [Column("org_id")]
        public Guid? OrgId { get; set; }
        [Column("business_id")]
        public Guid? BusinessId { get; set; }
        [Column("cus_id")]
        public Guid? CusId { get; set; }
        [Column("cus_firstname")]
        public string? CusConFirstname { get; set; }
        [Column("cus_lastname")]
        public string? CusConLastname { get; set; }
        [Column("cus_tel_no")]
        public string? TelNo { get; set; }
        [Column("cus_ext_no")]
        public string? ExtentNo { get; set; }
        [Column("cus_mobille_no")]
        public string? MobileNo { get; set; }
        [Column("cus_mail")]
        public string? Email { get; set; }
        [Column("cus_con_status")]
        public string? CusConStatus { get; set; }

    }
}
