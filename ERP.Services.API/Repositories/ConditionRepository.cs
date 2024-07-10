using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Repositories
{
    public class ConditionRepository : BaseRepository, IConditionRepository
    {
        public ConditionRepository(PromDbContext context) {
            this.context = context;
        }

        public IQueryable<ConditionEntity> GetConditionByBusiness(Guid orgId, Guid businessId)
        {
            return context.Conditions.Where(x => x.OrgId == orgId && x.BusinessId == businessId);
        }

        public void AddCondition(ConditionEntity query)
        {
            try
            {
                context.Conditions.Add(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateCondition(ConditionEntity query)
        {
            try
            {
                context.Conditions.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteCondition(ConditionEntity query)
        {
            try
            {
                query.ConditionStatus = RecordStatus.InActive.ToString();
                context.Conditions.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
