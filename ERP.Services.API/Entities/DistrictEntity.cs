using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Services.API.Entities
{
    [Table("Districts")]
    public class DistrictEntity
    {
        [Key]
        [Column("district_id")]
        public Guid? DistrictId { get; set; }

        [Column("province_code")]
        public int? ProvinceCode { get; set; }

        [Column("district_code")]
        public int? DistrictCode { get; set; }

        [Column("district_name_en")]
        public string? DistrictNameEn { get; set; }

        [Column("district_name_th")]
        public string? DistrictNameTh { get; set; }

        [Column("postal_code")]
        public string? PostalCode { get; set; }
    }
}
