﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("Project")]
    [Index(nameof(ProjectId), nameof(BusinessId), nameof(CustomerId), nameof(ProjectCustomId), IsUnique = true)]
    public class ProjectEntity
    {
        [Key]
        [Column("project_id")]
        public Guid ProjectId { get; set; }
        [Column("org_id")]
        public Guid? OrgId { get; set; }
        [Column("business_id")]
        public Guid? BusinessId { get; set; }
        [Column("cus_id")]
        public Guid? CustomerId { get; set; }
        [Column("user_id")]
        public Guid? UserId { get; set; }
        [Column("project_cus_Id")]
        public string? ProjectCustomId { get; set; }
        [Column("project_name")]
        public string? ProjectName { get; set; }
        [Column("project_status")]
        public string? ProjectStatus { get; set; }
    }
}
