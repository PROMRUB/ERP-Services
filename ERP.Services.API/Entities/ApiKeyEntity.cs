﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Services.API.Entities
{
    [Table("ApiKeys")]
    [Index(nameof(ApiKey), IsUnique = true)]
    [Index(nameof(OrgId))]
    public class ApiKeyEntity
    {
        public ApiKeyEntity()
        {
            KeyId = Guid.NewGuid();
            KeyCreatedDate = DateTime.UtcNow;
        }

        [Key]
        [Column("key_id")]
        public Guid? KeyId { get; set; }

        [Column("api_key")]
        public string? ApiKey { get; set; }

        [Column("org_id")]
        public string? OrgId { get; set; }

        [Column("key_created_date")]
        public DateTime? KeyCreatedDate { get; set; }

        [Column("key_expired_date")]
        public DateTime? KeyExpiredDate { get; set; }

        [Column("key_description")]
        public string? KeyDescription { get; set; }

        [Column("roles_list")]
        public string? RolesList { get; set; }
    }
}
