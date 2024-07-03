using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Models.ResponseModels.Project
{
    public class ProjectResponse
    {
        public Guid ProductCatId { get; set; }
        public Guid? OrgId { get; set; }
        public Guid? BusinessId { get; set; }
        public Guid CustomerId { get; set; }
        public string? ProjectCustomId { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectStatus { get; set; }
    }
}
