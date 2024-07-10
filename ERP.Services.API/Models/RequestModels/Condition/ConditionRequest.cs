using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Services.API.Models.RequestModels.Condition
{
    public class ConditionRequest
    {
        public Guid? ConditionId { get; set; }
        public Guid? OrgId { get; set; }
        public Guid? BusinessId { get; set; }
        public string? ConditionDescription { get; set; }
        public int? OrderBy { get; set; }
        public string? ConditionStatus { get; set; }
    }
}
