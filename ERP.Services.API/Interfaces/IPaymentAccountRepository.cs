using ERP.Services.API.Entities;
using ERP.Services.API.Enum;

namespace ERP.Services.API.Interfaces
{
    public interface IPaymentAccountRepository
    {
        public IQueryable<PaymentAccountEntity> GetPaymentAccountByBusiness(Guid orgId, Guid businessId);
        public void AddPaymentAccount(PaymentAccountEntity query);
        public void UpdatePaymentAccount(PaymentAccountEntity query);
        public void DeletePaymentAccount(PaymentAccountEntity query);
        public void Commit();
    }
}
