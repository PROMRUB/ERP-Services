using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("Businesses")]
    [Index(nameof(BusinessCustomId), IsUnique = true)]
    public class BusinessEntity
    {
        public BusinessEntity()
        {
            BusinessId = Guid.NewGuid();
            BusinessCreatedDate = DateTime.UtcNow;
        }


        [Key]
        [Column("business_id")]
        public Guid? BusinessId { get; set; }

        [Column("business_custom_id")]
        public string? BusinessCustomId { get; set; }

        [Column("org_id")]
        public Guid? OrgId { get; set; }

        [Column("business_type")]
        public string? BusinessType { get; set; }

        [Column("business_name")]
        public string? BusinessName { get; set; }

        [Column("display_name")]
        public string? DisplayName { get; set; }

        [Column("business_logo")]
        public string? BusinessLogo { get; set; }

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

        [Column("business_description")]
        public string? BusinessDescription { get; set; }

        [Column("business_created_date")]
        public DateTime? BusinessCreatedDate { get; set; }

        [Column("business_status")]
        public string? BusinessStatus { get; set; }

        public string Address()
        {
            return $"{No} {RoomNo} {Village} {Moo} {Alley} {SubDistrict} {District} {Road} {PostCode}";
        }
    }
}
