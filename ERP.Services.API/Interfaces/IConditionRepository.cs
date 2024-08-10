using ERP.Services.API.Entities;
using ERP.Services.API.Enum;

namespace ERP.Services.API.Interfaces
{
    public interface IConditionRepository
    {
        public IQueryable<ConditionEntity> GetConditionByBusiness(Guid orgId, Guid businessId);
        public void AddCondition(ConditionEntity query);
        public void UpdateCondition(ConditionEntity query);
        public void DeleteCondition(ConditionEntity query);
        public void Commit();
    }
}
