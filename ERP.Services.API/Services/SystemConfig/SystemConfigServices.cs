using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.ResponseModels.SystemConfig;
using ERP.Services.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Services.SystemConfig
{
    public class SystemConfigServices : ISystemConfigServices
    {
        private readonly IMapper mapper;
        private readonly ISystemConfigRepository repository;

        public SystemConfigServices(IMapper mapper,
            ISystemConfigRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<List<ProvinceResponse>> GetProvincesAsync()
        {
            var query = await repository.GetProvinceList().ToListAsync();
            return mapper.Map<List<ProvinceEntity>, List<ProvinceResponse>>(query);
        }

        public async Task<List<DistrictResponse>> GetDistrictsAsync()
        {
            var query = await repository.GetDistrictList().ToListAsync();
            return mapper.Map<List<DistrictEntity>, List<DistrictResponse>>(query);
        }

        public async Task<List<SubDIstrictResponse>> GetSubDIstrictsAsync()
        {
            var query = await repository.GetSubDistrictList().ToListAsync();
            return mapper.Map<List<SubDistrictEntity>, List<SubDIstrictResponse>>(query);
        }
    }
}
