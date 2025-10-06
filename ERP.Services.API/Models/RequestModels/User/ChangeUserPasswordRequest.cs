namespace ERP.Services.API.Models.RequestModels.User;

public class ChangeUserPasswordRequest
{
    public Guid OrgUserId { get; set; }
    public string NewPassword { get; set; } = default!;
}