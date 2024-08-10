namespace ERP.Services.API.Models.RequestModels.Organization
{
    public class OrganizationUserRequest
    {
        public Guid? OrgUserId { get; set; }
        public string? OrgCustomId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstNameTh { get; set; }
        public string? LastnameTh { get; set; }
        public string? Email { get; set; }
        public string? TelNo { get; set; }
    }
}
