using ERP.Services.API.Entities;

namespace ERP.Services.API.Interfaces
{
    public interface ISystemConfigRepository
    {
        public IQueryable<ProvinceEntity> GetProvinceList();
        public IQueryable<DistrictEntity> GetDistrictList();
        public IQueryable<SubDistrictEntity> GetSubDistrictList();
    }
}
