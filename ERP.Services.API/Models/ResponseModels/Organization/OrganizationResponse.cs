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
    }
}
