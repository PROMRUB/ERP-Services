using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Organization;
using ERP.Services.API.Models.ResponseModels.Organization;
using ERP.Services.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Services.Organization
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IBusinessRepository businessRepository;
        private readonly IUserService userService;
        private readonly ISystemConfigRepository systemRepository;

        public OrganizationService(IMapper mapper,
            IConfiguration configuration,
            IOrganizationRepository organizationRepository,
            IBusinessRepository businessRepository,
            IUserService userService,
            ISystemConfigRepository systemRepository) : base()
        {
            this.mapper = mapper;
            this.configuration = configuration;
            this.organizationRepository = organizationRepository;
            this.businessRepository = businessRepository;
            this.userService = userService;
            this.systemRepository = systemRepository;
        }

        public async Task<bool> AddOrganization(string orgId, OrganizationRequest org)
        {
            var orgData = await organizationRepository.OrganizationNumberAsync();
            org.OrgCustomId = "PBID" + orgData.OrgDate + "." + orgData.Allocated!.Value.ToString("D5")+".TH";
            var customOrgId = org.OrgCustomId;
            if (organizationRepository!.IsCustomOrgIdExist(customOrgId!))
                throw new ArgumentException("1111");
            organizationRepository!.SetCustomOrgId(customOrgId!);
            var organizationRequest = mapper.Map<OrganizationRequest, OrganizationEntity>(org);
            await organizationRepository!.AddOrganization(organizationRequest);
            var businessRequest = mapper.Map<OrganizationRequest, BusinessEntity>(org);
            businessRequest.OrgId = organizationRequest.OrgId;
            await businessRepository!.AddBusiness(businessRequest);
            return true;
        }

        public async Task<bool> AddBusiness(string orgId, OrganizationRequest org)
        {
            organizationRepository!.SetCustomOrgId(orgId!);
            var orgDetail = await organizationRepository.GetOrganization();
            var customerBusiness = org.TaxId + "." + org.BrnId;
            if (businessRepository!.IsCustomBusinessIdExist(customerBusiness))
                throw new ArgumentException("1111");
            var businessRequest = mapper.Map<OrganizationRequest, BusinessEntity>(org);
            businessRequest.OrgId = orgDetail.OrgId;
            await businessRepository!.AddBusiness(businessRequest);
            return true;
        }

        public async Task<bool> UpdateOrganization(string orgId, OrganizationRequest org)
        {
            var customOrgId = org.OrgCustomId;
            organizationRepository!.SetCustomOrgId(customOrgId!);
            var orgDetail = await organizationRepository.GetOrganization();
            orgDetail.DisplayName = org.DisplayName;
            orgDetail.OrgLogo = org.OrgLogo;
            organizationRepository.Commit();
            return true;
        }
        public async Task<bool> UpdateSecurity(string orgId, OrganizationRequest org)
        {
            var customOrgId = org.OrgCustomId;
            organizationRepository!.SetCustomOrgId(customOrgId!);
            var orgDetail = await organizationRepository.GetOrganization();
            organizationRepository.Commit();
            return true;
        }

        public async Task<OrganizationResponse> GetOrganization(string orgId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var query = await organizationRepository!.GetOrganization();
            return mapper.Map<OrganizationEntity, OrganizationResponse>(query);
        }

        public async Task<List<OrganizationResponse>> GetBusiness(string orgId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var orgQuery = await organizationRepository!.GetOrganization();
            businessRepository!.SetCustomOrgId(orgId);
            var query = await businessRepository!.GetBusinesses((Guid)orgQuery.OrgId).ToListAsync();
            return mapper.Map<List<BusinessEntity>, List<OrganizationResponse>>(query);
        }

        public async Task<OrganizationResponse> GetBusinessById(string orgId,string businessId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var orgQuery = await organizationRepository!.GetOrganization();
            businessRepository!.SetCustomOrgId(orgId);
            var query = await businessRepository!.GetBusinesses((Guid)orgQuery.OrgId).Where(x => x.BusinessCustomId.Equals(businessId)).FirstOrDefaultAsync();
            var result = mapper.Map<BusinessEntity, OrganizationResponse>(query);
            var test = systemRepository.GetDistrictList().Where(x => x.DistrictCode.ToString().Equals(query.District)).FirstOrDefault();
            result.OrgAddress = (string.IsNullOrEmpty(query.Building) ? "" : (query.Building + " ")) +
                                (string.IsNullOrEmpty(query.RoomNo) ? "" : (query.RoomNo + " ")) +
                                (string.IsNullOrEmpty(query.Floor) ? "" : (query.Floor + " ")) +
                                (string.IsNullOrEmpty(query.Village) ? "" : (query.Village + " ")) +
                                (string.IsNullOrEmpty(query.No) ? "" : (query.No + " ")) +
                                (string.IsNullOrEmpty(query.Moo) ? "" : (query.Moo + " ")) +
                                (string.IsNullOrEmpty(query.Alley) ? "" : (query.Alley + " ")) +
                                (string.IsNullOrEmpty(query.Road) ? "" : (query.Road + " ")) +
                                (string.IsNullOrEmpty(query.SubDistrict) ? "" : (systemRepository.GetSubDistrictList().Where(x => x.SubDistrictCode.ToString().Equals(query.SubDistrict)).FirstOrDefault().SubDistrictNameTh + " ")) +
                                (string.IsNullOrEmpty(query.District) ? "" : (systemRepository.GetDistrictList().Where(x => x.DistrictCode.ToString().Equals(query.District)).FirstOrDefault().DistrictNameTh + " ")) +
                                (string.IsNullOrEmpty(query.Provice) ? "" : (systemRepository.GetProvinceList().Where(x => x.ProvinceCode.ToString().Equals(query.Provice)).FirstOrDefault().ProvinceNameTh + " ")) +
                                (string.IsNullOrEmpty(query.PostCode) ? "" : query.PostCode);
            return result;
        }

        public async Task<List<OrganizationUserResponse>> GetUserAllowedOrganization(string userName)
        {
            var query = await organizationRepository!.GetUserAllowedOrganizationAsync(userName);
            return mapper.Map<IEnumerable<OrganizationUserEntity>, List<OrganizationUserResponse>>(query);
        }

        public bool IsUserNameExist(string orgId, string userName)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var result = organizationRepository!.IsUserNameExist(userName);
            return result;
        }

        public void AddUserToOrganization(string orgId, OrganizationUserRequest user)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            if (userService.IsUserNameExist(orgId, user!.Username!))
                throw new ArgumentException("1111");
            var request = mapper.Map<OrganizationUserRequest, OrganizationUserEntity>(user);
            organizationRepository!.AddUserToOrganization(request);
        }

        public async Task<bool> VerifyUserInOrganization(string orgId, string userName)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            if (await userService.GetUserByName(orgId, userName) == null || await organizationRepository!.GetUserInOrganization(userName) == null)
                throw new KeyNotFoundException("1102");
            return true;
        }
    }
}
