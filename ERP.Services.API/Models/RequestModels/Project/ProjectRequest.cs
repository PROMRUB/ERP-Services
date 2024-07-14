namespace ERP.Services.API.Models.RequestModels.Project
{
    public class ProjectRequest
    {
        public Guid ProjectId { get; set; }
        public Guid? OrgId { get; set; }
        public Guid? BusinessId { get; set; }
        public Guid CustomerId { get; set; }
        public string? ProjectCustomId { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectStatus { get; set; }
    }
}
