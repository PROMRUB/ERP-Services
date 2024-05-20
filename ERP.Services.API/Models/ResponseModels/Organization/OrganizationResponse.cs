namespace ERP.Services.API.Models.ResponseModels.Organization
{
    public class OrganizationResponse
    {
        public Guid? OrgId { get; set; }
        public string? OrgCustomId { get; set; }
        public string? OrgName { get; set; }
        public string? OrgDescription { get; set; }
        public string? OrgAddress { get; set; }
    }
}
