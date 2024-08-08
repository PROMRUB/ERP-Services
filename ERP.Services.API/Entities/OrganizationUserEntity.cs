using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ERP.Services.API.Entities
{
    [Table("OrganizationsUsers")]
    public class OrganizationUserEntity
    {
        public OrganizationUserEntity()
        {
            OrgUserId = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [Column("org_user_id")]
        public Guid? OrgUserId { get; set; }

        [Column("org_custom_id")]
        public string? OrgCustomId { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("first_name_th")]
        public string? FirstNameTh { get; set; }

        [Column("last_name_th")]
        public string? LastnameTh { get; set; }

        [Column("email")]
        public string? email { get; set; }

        [Column("tel_no")]
        public string? TelNo { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        public string DisplayNameTH()
        {
            return $"{FirstNameTh} {LastnameTh}";
        }
        
    }
}
