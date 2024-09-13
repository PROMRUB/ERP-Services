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
using ERP.Services.API.Utils;

namespace ERP.Services.API.Services.PaymentAccount
{
    public class PaymentAccountService : BaseService, IPaymentAccountService
    {
        private readonly IMapper mapper;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IPaymentAccountRepository paymentAccountRepository;
        private readonly ISystemConfigRepository systemConfigRepository;

        public PaymentAccountService(IMapper mapper,
            IOrganizationRepository organizationRepository,
            IPaymentAccountRepository paymentAccountRepository,
            ISystemConfigRepository systemConfigRepository)
        {
            this.mapper = mapper;
            this.organizationRepository = organizationRepository;
            this.paymentAccountRepository = paymentAccountRepository;
            this.systemConfigRepository = systemConfigRepository;
        }

        public async Task<List<PaymentAccountResponse>> GetPaymentAccountListByBusiness(string orgId, Guid businessId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId)
                .Include(x => x.BankEntity)
                .Include(x => x.BankBranchEntity)
                .Where(x => x.AccountStatus == RecordStatus.Active.ToString())
                .OrderBy(x => x.BankId)
                .ToListAsync();
            var result = query.Select(x => new PaymentAccountResponse
                {
                    PaymentAccountId = x.PaymentAccountId,
                    OrgId = x.OrgId,
                    PaymentAccountName = x.PaymentAccountName,
                    AccountType = x.AccountType,
                    AccountBank = x.BankId,
                    AccountBankName = x.BankEntity.BankTHName,
                    AccountBrn = x.BankBranchId,
                    AccountBankBrn = x.BankBranchEntity.BankBranchTHName,
                    PaymentAccountNo = x.PaymentAccountNo,
                    AccountStatus = x.AccountStatus
                })
                .ToList();
            // var result = mapper.Map<List<PaymentAccountEntity>, List<PaymentAccountResponse>>(query);
            var bankQuery = await systemConfigRepository.GetAll<BankEntity>().ToListAsync();
            foreach (var item in result)
            {
                var bank = bankQuery.Where(x => x.BankId == item.AccountBank).FirstOrDefault();
                item.AccountBankName = bank.BankTHName;
                var branch = await systemConfigRepository.GetAll<BankBranchEntity>().Where(x => x.BankCode.Equals(bank.BankCode)).ToListAsync();
                item.AccountBankBrn = branch.Where(x => x.BankBranchId == item.AccountBrn).FirstOrDefault().BankBranchTHName;
            }
            return result;
        } 
        
        public async Task<PagedList<PaymentAccountResponse>> GetPaymentAccountListByBusiness(string orgId, Guid businessId,string keyword,int page,int pageSize)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query =  paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId)
                .Include(x => x.BankEntity)
                .Include(x => x.BankBranchEntity)
                .Where(x => x.AccountStatus == RecordStatus.Active.ToString())
                .OrderBy(x => x.BankId);
            // var result = mapper.Map<List<PaymentAccountEntity>, List<PaymentAccountResponse>>(query);
            // var bankQuery = await systemConfigRepository.GetAll<BankEntity>().ToListAsync();
            // foreach (var item in result)
            // {
            //     var bank = bankQuery.Where(x => x.BankId == item.AccountBank).FirstOrDefault();
            //     item.AccountBankName = bank.BankTHName;
            //     var branch = await systemConfigRepository.GetAll<BankBranchEntity>().Where(x => x.BankCode.Equals(bank.BankCode)).ToListAsync();
            //     item.AccountBankBrn = branch.Where(x => x.BankBranchId == item.AccountBrn).FirstOrDefault().BankBranchTHName;
            // }

            var result = query.Select(x => new PaymentAccountResponse
            {
                PaymentAccountId = x.PaymentAccountId,
                OrgId = x.OrgId,
                PaymentAccountName = x.PaymentAccountName,
                AccountType = x.AccountType,
                AccountBankName = x.BankEntity.BankTHName,
                AccountBankBrn = x.BankBranchEntity.BankBranchTHName,
                PaymentAccountNo = x.PaymentAccountNo,
                AccountStatus = x.AccountStatus == "Active" ? "ปกติ" : "รออนุมติ"
            });

            var paged = await PagedList<PaymentAccountResponse>.Create(result, page, pageSize);
            
            return paged;
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
            query.BankId = request.AccountBank;
            query.BankBranchId = request.AccountBrn;
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
