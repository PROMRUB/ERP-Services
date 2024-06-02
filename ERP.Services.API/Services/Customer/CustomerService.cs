using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.ResponseModels.Customer;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ERP.Services.API.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper mapper;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrganizationRepository organizationRepository;
        public CustomerService(IMapper mapper,
            ICustomerRepository customerRepository,
            IOrganizationRepository organizationRepository)
        {
            this.mapper = mapper;
            this.customerRepository = customerRepository;
            this.organizationRepository = organizationRepository;
        }

        public async Task<List<CustomerResponse>> GetCustomerByBusinessAsync(string orgId, Guid businessId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId).Where(x => x.CusStatus == RecordStatus.Active.ToString()).ToListAsync();
            return mapper.Map<List<CustomerEntity>, List<CustomerResponse>>(result);
        }

        public async Task<CustomerResponse> GetCustomerInformationByIdAsync(string orgId, Guid businessId, Guid customerId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId).Where(x => x.CusId == customerId).FirstOrDefaultAsync();
            return mapper.Map<CustomerEntity, CustomerResponse>(result);
        }

        public async Task CreateCustomer(string orgId, CustomerRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = mapper.Map<CustomerRequest, CustomerEntity>(request);
            string cleanedCusNameEng = Regex.Replace(request.CusNameEng, "[^a-zA-Z0-9]+", "");
            char firstCharacter = cleanedCusNameEng.ToUpper().FirstOrDefault();
            var runNo = await customerRepository.CustomerNumberAsync((Guid)organization.OrgId, (Guid)request.BusinessId, firstCharacter.ToString());
            query.OrgId = organization.OrgId;
            query.CusCustomId = "C." + runNo.Character + "-" + runNo.Allocated.Value.ToString("D5") + ".D" ;
            customerRepository.CreateCustomer(query);
            customerRepository.Commit();
        }
        public async Task UpdateCustomer(string orgId, Guid businessId, Guid customerId, CustomerRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId).Where(x => x.CusId == customerId).FirstOrDefaultAsync();
            if (query == null)
                throw new ArgumentException("1111");
            query.CusType = request.CusType;
            query.CusName = request.CusName;
            query.CusNameEng = request.CusNameEng;
            query.DisplayName = request.DisplayName;
            query.Building = request.Building;
            query.RoomNo = request.RoomNo;
            query.Floor = request.Floor;
            query.Village = request.Village;
            query.Moo = request.Moo;
            query.No = request.No;
            query.Road = request.Road;
            query.Alley = request.Alley;
            query.Province = request.Province;
            query.District = request.District;
            query.SubDistrict = request.SubDistrict;
            query.PostCode = request.PostCode;
            query.Website = request.Website;
            customerRepository.UpdateCustomer(query);
            customerRepository.Commit();
        }

        public async Task DeleteCustomer(string orgId, List<CustomerRequest> request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            foreach (var customer in request)
            {
                var query = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, (Guid)customer.BusinessId).Where(x => x.CusId == customer.CusId).FirstOrDefaultAsync();
                customerRepository.DeleteCustomer(query);
            }
            customerRepository.Commit();
        }
    }
}
