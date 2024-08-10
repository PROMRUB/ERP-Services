using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Models.ResponseModels.Condition
{
    public class ConditionResponse
    {
        public Guid? ConditionId { get; set; }
        public Guid? OrgId { get; set; }
        public Guid? BusinessId { get; set; }
        public string? ConditionDescription { get; set; }
        public int? OrderBy { get; set; }
        public string? ConditionStatus { get; set; }
    }
}
