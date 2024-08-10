using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Services.API.Entities
{
    [Table("Provinces")]
    public class ProvinceEntity
    {
        [Key]
        [Column("province_id")]
        public Guid? ProvinceId { get; set; }
        [Column("province_code")]
        public int? ProvinceCode { get; set; }
        [Column("province_name_en")]
        public string? ProvinceNameEn { get; set; }
        [Column("province_name_th")]
        public string? ProvinceNameTh { get; set; }
    }
}
