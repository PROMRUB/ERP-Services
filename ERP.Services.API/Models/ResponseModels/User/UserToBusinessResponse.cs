namespace ERP.Services.API.Models.ResponseModels.User;

public class UserToBusinessResponse
{
    public Guid? OrgId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? BusinessId { get; set; }
    public string? Role { get; set; }
    public int EmployeeRunning { get; set; }
    public string? EmployeeCode { get; set; }
}