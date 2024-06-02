using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Services.API.Entities
{
    [Table("Organizations")]
    [Index(nameof(OrgCustomId), IsUnique = true)]
    public class OrganizationEntity
    {
        public OrganizationEntity()
        {
            OrgId = Guid.NewGuid();
            OrgCreatedDate = DateTime.UtcNow;
        }

        [Key]
        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("org_custom_id")]
        public string? OrgCustomId { get; set; }

        [Column("business_type")]
        public string? BusinessType { get; set; }

        [Column("org_name")]
        public string? OrgName { get; set; }

        [Column("display_name")]
        public string? DisplayName { get; set; }

        [Column("org_logo")]
        public string? OrgLogo { get; set; }

        [Column("tax_id")]
        public string? TaxId { get; set; }

        [Column("branch_id")]
        public string? BrnId { get; set; }

        [Column("building")]
        public string? Building { get; set; }

        [Column("room_no")]
        public string? RoomNo { get; set; }

        [Column("floor")]
        public string? Floor { get; set; }

        [Column("village")]
        public string? Village { get; set; }

        [Column("moo")]
        public string? Moo { get; set; }

        [Column("house_no")]
        public string? No { get; set; }

        [Column("road")]
        public string? Road { get; set; }

        [Column("alley")]
        public string? Alley { get; set; }

        [Column("province")]
        public string? Province { get; set; }

        [Column("district")]
        public string? District { get; set; }

        [Column("sub_district")]
        public string? SubDistrict { get; set; }

        [Column("post_code")]
        public string? PostCode { get; set; }

        [Column("website")]
        public string? Website { get; set; }

        [Column("org_description")]
        public string? OrgDescription { get; set; }

        [Column("org_created_date")]
        public DateTime? OrgCreatedDate { get; set; }

        [Column("org_status")]
        public string? OrgStatus { get; set; }
    }
}
