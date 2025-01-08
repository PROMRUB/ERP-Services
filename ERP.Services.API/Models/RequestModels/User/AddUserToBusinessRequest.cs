namespace ERP.Services.API.Models.RequestModels.User
{
    public class AddUserToBusinessRequest
    {
        public Guid UserId { get; set; }
        public Guid BusinessId { get; set; }
        public string? Roles { get; set; }
    }
}
