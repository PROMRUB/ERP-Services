using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Repositories
{
    public class PaymentAccountRepository : BaseRepository, IPaymentAccountRepository
    {
        public PaymentAccountRepository(PromDbContext context) {
            this.context = context;
        }

        public IQueryable<PaymentAccountEntity> GetPaymentAccountByBusiness(Guid orgId, Guid businessId)
        {
            return context.PaymentAccounts.Where(x => x.OrgId == orgId && x.BusinessId == businessId);
        }

        public void AddPaymentAccount(PaymentAccountEntity query)
        {
            try
            {
                context.PaymentAccounts.Add(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdatePaymentAccount(PaymentAccountEntity query)
        {
            try
            {
                context.PaymentAccounts.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeletePaymentAccount(PaymentAccountEntity query)
        {
            try
            {
                query.AccountStatus = RecordStatus.InActive.ToString();
                context.PaymentAccounts.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
