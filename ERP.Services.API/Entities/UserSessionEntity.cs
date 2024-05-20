using ERP.Services.API.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("UserSession")]
    [Index(nameof(UserSessionId), IsUnique = true)]
    public class UserSessionEntity
    {
        public UserSessionEntity()
        {
            UserSessionId = Guid.NewGuid();
            SessionStatus = RecordStatus.Active.ToString();
        }

        [Key]
        [Column("user_session_id")]
        public Guid UserSessionId { get; set; }

        [Column("userId")]
        public Guid? UserId { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("session_status")]
        public string SessionStatus { get; set; }
    }
}
