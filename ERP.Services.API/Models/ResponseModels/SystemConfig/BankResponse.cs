using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Services.API.Models.ResponseModels.SystemConfig
{
    public class BankResponse
    {
        public Guid? BankId { get; set; }
        public string? BankCode { get; set; }
        public string? BankAbbr { get; set; }
        public string? BankTHName { get; set; }
        public string? BankENName { get; set; }
    }
}
