namespace ERP.Services.API.Models.RequestModels.Product
{
    public class ProductCategoryRequest
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
