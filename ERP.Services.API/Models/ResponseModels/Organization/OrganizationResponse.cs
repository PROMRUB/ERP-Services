namespace ERP.Services.API.Models.ResponseModels.Organization
{
    public class OrganizationResponse
    {
        public Guid? OrgId { get; set; }
        public string? TaxId { get; set; }
        public string? BrnId { get; set; }
        public string? OrgCustomId { get; set; }
        public string? OrgName { get; set; }
        public string? OrgDescription { get; set; }
        public string? OrgAddress { get; set; }
        public string? HotLine { get; set; }
        public string? Url { get; set; }

        public string? Email { get; set; }
        public string? Tel { get; set; }

        // ===== เพิ่มให้ ครอบคลุม BusinessEntity =====
        public Guid? BusinessId { get; set; }
        public string? BusinessCustomId { get; set; }
        public string? BusinessType { get; set; }

        public string? DisplayName { get; set; }
        public string? OrgLogo { get; set; } // map จาก BusinessLogo หรือ Logo

        // Address parts
        public string? Building { get; set; }
        public string? RoomNo { get; set; }
        public string? Floor { get; set; }
        public string? Village { get; set; }
        public string? Moo { get; set; }
        public string? No { get; set; }
        public string? Road { get; set; }
        public string? Alley { get; set; }

        public string? Province { get; set; }
        public string? District { get; set; } 
        public string? SubDistrict { get; set; } 
        public string? PostCode { get; set; }

        public string? Website { get; set; }

        public string? ProvinceNameTh { get; set; }
        public string? DistrictNameTh { get; set; }
        public string? SubDistrictNameTh { get; set; }
    }
}