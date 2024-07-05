using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Models.ResponseModels.Customer
{
    public class CustomerContactResponse
    {
        public Guid? CusConId { get; set; }
        public Guid? OrgId { get; set; }
        public Guid? BusinessId { get; set; }
        public Guid? CusId { get; set; }
        public string? CusConFirstname { get; set; }
        public string? CusConLastname { get; set; }
        public string? CusConName { get; set; }
        public string? TelNo { get; set; }
        public string? ExtentNo { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? CusConStatus { get; set; }
    }
}
