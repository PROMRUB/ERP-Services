using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Entities
{
    [Table("Customer")]
    [Index(nameof(CusId), nameof(CusCustomId), nameof(OrgId), nameof(BusinessId), IsUnique = true)]
    public class CustomerEntity
    {
        public CustomerEntity()
        {
            CusId = Guid.NewGuid();
            CusCreatedDate = DateTime.UtcNow;
        }

        [Key] [Column("cus_id")] public Guid? CusId { get; set; }

        [Column("cus_custom_id")] public string? CusCustomId { get; set; }

        [Column("org_id")] public Guid? OrgId { get; set; }

        [Column("business_id")] public Guid? BusinessId { get; set; }

        [Column("cus_type")] public string? CusType { get; set; }

        [Column("cus_name")] public string? CusName { get; set; }

        [Column("cus_name_eng")] public string? CusNameEng { get; set; }

        [Column("display_name")] public string? DisplayName { get; set; }

        [Column("tax_id")] public string? TaxId { get; set; }

        [Column("branch_id")] public string? BrnId { get; set; }

        [Column("building")] public string? Building { get; set; }

        [Column("room_no")] public string? RoomNo { get; set; }

        [Column("floor")] public string? Floor { get; set; }

        [Column("village")] public string? Village { get; set; }

        [Column("moo")] public string? Moo { get; set; }

        [Column("house_no")] public string? No { get; set; }

        [Column("road")] public string? Road { get; set; }

        [Column("alley")] public string? Alley { get; set; }

        [Column("province")] public string? Province { get; set; }

        [Column("district")] public string? District { get; set; }

        [Column("sub_district")] public string? SubDistrict { get; set; }

        [Column("post_code")] public string? PostCode { get; set; }

        [Column("website")] public string? Website { get; set; }

        [Column("cus_description")] public string? CusDescription { get; set; }

        [Column("cus_created_date")] public DateTime? CusCreatedDate { get; set; }

        [Column("cus_status")] public string? CusStatus { get; set; }

        public string Address()
        {
            var address = "";

            if (!string.IsNullOrEmpty(RoomNo))
            {
                address += $"ห้องเลขที่ {RoomNo} ";
            }
            
            if (!string.IsNullOrEmpty(Floor))
            {
                address += $"ชั้น {Floor} ";
            }
            
            if (!string.IsNullOrEmpty(Building))
            {
                address += $"อาคาร {Building} ";
            }
            
            if (!string.IsNullOrEmpty(Village))
            {
                address += $"หมู่บ้าน {Building} ";
            }
            
            if (!string.IsNullOrEmpty(Alley))
            {
                address += $"ซอย {Alley} ";
            }
            
            if (!string.IsNullOrEmpty(Alley))
            {
                address += $"ซอย {Alley} ";
            }
            
            if (!string.IsNullOrEmpty(SubDistrict))
            {
                address += $"ตำบล {SubDistrict} ";
            }
            
            if (!string.IsNullOrEmpty(District))
            {
                address += $"อำเภอ {District} ";
            }
            
            if (!string.IsNullOrEmpty(Road))
            {
                address += $"ถนน {Road} ";
            }
            
            if (!string.IsNullOrEmpty(PostCode))
            {
                address += $"รหัสไปรษณีย์ {PostCode} ";
            }
            
            return address;
        }
    }
}