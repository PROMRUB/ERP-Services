using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;

namespace ERP.Services.API.Repositories
{
    public class SystemConfigRepository : BaseRepository, ISystemConfigRepository
    {
        public SystemConfigRepository(PromDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ProvinceEntity> GetProvinceList()
        {
            return context.Provinces;
        }

        public IQueryable<DistrictEntity> GetDistrictList()
        {
            return context.District;
        }

        public IQueryable<SubDistrictEntity> GetSubDistrictList()
        {
            return context.SubDistrict;
        }
    }
}
