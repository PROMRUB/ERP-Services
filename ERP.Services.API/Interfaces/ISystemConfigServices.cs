using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Models.ResponseModels.SystemConfig;

namespace ERP.Services.API.Interfaces
{
    public interface ISystemConfigServices
    {
        public Task<List<ProvinceResponse>> GetProvincesAsync();

        public Task<List<DistrictResponse>> GetDistrictsAsync();

        public Task<List<SubDIstrictResponse>> GetSubDIstrictsAsync();
    }
}
