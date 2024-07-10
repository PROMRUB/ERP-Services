using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Condition;
using ERP.Services.API.Models.RequestModels.PaymentAccount;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.RequestModels.Project;
using ERP.Services.API.Models.ResponseModels.Condition;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Product;
using ERP.Services.API.Models.ResponseModels.Project;
using ERP.Services.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ERP.Services.API.Services.PaymentAccount
{
    public class PaymentAccountService : BaseService, IPaymentAccountService
    {
        private readonly IMapper mapper;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IPaymentAccountRepository paymentAccountRepository;

        public PaymentAccountService(IMapper mapper,
            IOrganizationRepository organizationRepository,
            IPaymentAccountRepository paymentAccountRepository)
        {
            this.mapper = mapper;
            this.organizationRepository = organizationRepository;
            this.paymentAccountRepository = paymentAccountRepository;
        }

        public async Task<List<PaymentAccountResponse>> GetPaymentAccountListByBusiness(string orgId, Guid businessId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId).Where(x => x.AccountStatus == RecordStatus.Active.ToString()).OrderBy(x => x.AccountBank).ToListAsync();
            return mapper.Map<List<PaymentAccountEntity>, List<PaymentAccountResponse>>(result);
        }

        public async Task<PaymentAccountResponse> GetPaymentAccountInformationById(string orgId, Guid businessId, Guid paymentAccountId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId).Where(x => x.PaymentAccountId == paymentAccountId).FirstOrDefaultAsync();
            return mapper.Map<PaymentAccountEntity, PaymentAccountResponse>(result);
        }

        public async Task CreatePaymentAccount(string orgId, PaymentAccountRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = mapper.Map<PaymentAccountRequest, PaymentAccountEntity>(request);
            request.OrgId = organization.OrgId;
            paymentAccountRepository.AddPaymentAccount(query);
            paymentAccountRepository.Commit();
        }
        public async Task UpdatePaymentAccount(string orgId, Guid businessId, Guid paymentAccountId, PaymentAccountRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId).Where(x => x.PaymentAccountId == paymentAccountId).FirstOrDefaultAsync();
            query.PaymentAccountName = request.PaymentAccountName;
            query.PaymentAccountNo = request.PaymentAccountNo;
            query.AccountBank = request.AccountBank;
            query.AccountBrn = request.AccountBrn;
            paymentAccountRepository.UpdatePaymentAccount(query);
            paymentAccountRepository.Commit();
        }

        public async Task DeletePaymentAccount(string orgId, Guid businessId, Guid paymentAccountId, PaymentAccountRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId).Where(x => x.PaymentAccountId == paymentAccountId).FirstOrDefaultAsync();
            paymentAccountRepository.DeletePaymentAccount(query);
            paymentAccountRepository.Commit();
        }
    }
}
