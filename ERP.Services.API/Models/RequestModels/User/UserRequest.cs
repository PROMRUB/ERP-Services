namespace ERP.Services.API.Models.RequestModels.User
{
    public class UserRequest
    {
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Tel { get; set; }
    }
}
