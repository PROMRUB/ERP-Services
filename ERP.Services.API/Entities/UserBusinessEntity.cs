using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ERP.Services.API.Entities
{
    [Table("UserBusiness")]
    [Index(nameof(UserBusinessId), IsUnique = true)]
    public class UserBusinessEntity
    {
        [Key]
        [Column("user_business_id")]
        public Guid? UserBusinessId { get; set; }

        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Column("business_id")]
        public Guid? BusinessId { get; set; }
    }
}
