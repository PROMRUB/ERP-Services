using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Models.ResponseModels.Product
{
    public class ProductCategoryResponse
    {
        public Guid ProductCatId { get; set; }
        public Guid? OrgId { get; set; }
        public Guid? BusinessId { get; set; }
        public string? CustomCatId { get; set; }
        public string? ParentCatId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryStatus { get; set; }
    }
}
