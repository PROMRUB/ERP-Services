using System.IdentityModel.Tokens.Jwt;

namespace ERP.Services.API.Models.ResponseModels.Organization
{
    public class OrganizationUserResponse
    {
        public Guid? OrgUserId { get; set; }
        public string? OrgCustomId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? TelNo { get; set; }
    }
}
