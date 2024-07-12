using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Models.ResponseModels.SystemConfig;
using OfficeOpenXml;

namespace ERP.Services.API.Interfaces
{
    public interface ISystemConfigServices
    {
        public Task<List<ProvinceResponse>> GetProvincesAsync();

        public Task<List<DistrictResponse>> GetDistrictsAsync();

        public Task<List<SubDIstrictResponse>> GetSubDistrictsAsync();

        public Task<List<BankResponse>> GetBanksAsync();

        public Task<List<BankBranchResponse>> GetBankBranchsAsync();

        public Task ImportBank(IFormFile request);

        public Task ImportBankBranch(IFormFile request);
    }
}
