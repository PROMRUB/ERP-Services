using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Services.API.Models.ResponseModels.SystemConfig
{
    public class BankBranchResponse
    {
        public Guid BankBranchId { get; set; }
        public string? BankCode { get; set; }
        public string? BankBranchCode { get; set; }
        public string? BankBranchTHName { get; set; }
        public string? BankBranchENName { get; set; }
    }
}
