using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Organization;
using ERP.Services.API.Models.ResponseModels.Organization;
using ERP.Services.API.Utils;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.FormulaExpressions;

namespace ERP.Services.API.Services.Organization
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IBusinessRepository businessRepository;
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;
        private readonly ISystemConfigRepository systemRepository;

        public OrganizationService(IMapper mapper,
            IConfiguration configuration,
            IOrganizationRepository organizationRepository,
            IBusinessRepository businessRepository,
            IUserService userService,
            IUserRepository userRepository,
            ISystemConfigRepository systemRepository,
            UserPrincipalHandler userPrincipalHandler) : base()
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
            org.OrgCustomId = "PBID" + orgData.OrgDate + "." + orgData.Allocated!.Value.ToString("D5") + ".TH";
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
        
        public async Task<bool> UpdateBusiness(string orgId, string businessId, OrganizationRequest req)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var orgDetail = await organizationRepository.GetOrganization();

            businessRepository!.SetCustomOrgId(orgId);
            var entity = await businessRepository!.GetBusinesses()
                .Where(x => x.BusinessCustomId == businessId)
                .FirstOrDefaultAsync();

            if (entity == null)
                throw new ArgumentException("ไม่พบข้อมูลธุรกิจ");

            // อัปเดตเฉพาะฟิลด์ที่ส่งมา (ไม่ทับด้วย null)
            void SetIf(string? v, Action<string> setter)
            {
                if (v != null) setter(v);
            }

            // core
            SetIf(req.BusinessType, v => entity.BusinessType = v);
            SetIf(req.DisplayName, v => entity.BusinessName = v);
            SetIf(req.DisplayName, v => entity.DisplayName = v);
            SetIf(req.OrgLogo, v => entity.BusinessLogo = v);

            SetIf(req.TaxId, v => entity.TaxId = v);
            SetIf(req.BrnId, v => entity.BrnId = v);

            // address parts
            SetIf(req.Building, v => entity.Building = v);
            SetIf(req.RoomNo, v => entity.RoomNo = v);
            SetIf(req.Floor, v => entity.Floor = v);
            SetIf(req.Village, v => entity.Village = v);
            SetIf(req.Moo, v => entity.Moo = v);
            SetIf(req.No, v => entity.No = v);
            SetIf(req.Road, v => entity.Road = v);
            SetIf(req.Alley, v => entity.Alley = v);

            SetIf(req.Province, v => entity.Province = v);
            SetIf(req.District, v => entity.District = v);
            SetIf(req.SubDistrict, v => entity.SubDistrict = v);
            SetIf(req.PostCode, v => entity.PostCode = v);

            // contact/desc
            SetIf(req.Website, v =>
            {
                entity.Website = v;
                entity.Url = v;
            }); // ให้สอดคล้องกับฝั่ง FE
            SetIf(req.OrgDescription, v => entity.BusinessDescription = v);

            // อัปเดต BusinessCustomId ใหม่ ถ้า taxId/brnId เปลี่ยน (ทั้ง 2 ฟิลด์ต้องมีค่า)
            if (req.TaxId != null || req.BrnId != null)
            {
                var tax = req.TaxId ?? entity.TaxId ?? "";
                var brn = req.BrnId ?? entity.BrnId ?? "";
                entity.BusinessCustomId = tax + "." + brn;
            }

            await businessRepository.UpdateBusiness(entity);
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

        public async Task<OrganizationResponse> GetOrganizationByTaxId(string orgId, string taxId, string brnId)
        {
            var query = await organizationRepository!.GetOrganizationList()
                .Where(x => x.TaxId.Equals(taxId) && x.BrnId.Equals(brnId)).FirstOrDefaultAsync();
            return mapper.Map<OrganizationEntity, OrganizationResponse>(query);
        }

        public async Task<List<OrganizationResponse>> GetBusiness(string orgId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var orgQuery = await organizationRepository!.GetOrganization();
            businessRepository!.SetCustomOrgId(orgId);
            var query = await businessRepository!.GetBusinesses().Join(
                (await userService.GetUserBusiness(orgId)),
                business => business.BusinessId,
                user => user.BusinessId,
                (business, user) => new OrganizationEntity
                {
                    OrgId = business.BusinessId,
                    OrgCustomId = business.BusinessCustomId,
                    DisplayName = business.BusinessName,
                }).ToListAsync();
            return mapper.Map<List<OrganizationEntity>, List<OrganizationResponse>>(query);
        }

        public async Task<OrganizationResponse> GetBusinessById(string orgId, string businessId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var orgQuery = await organizationRepository!.GetOrganization();
            businessRepository!.SetCustomOrgId(orgId);
            var query = await businessRepository!.GetBusinesses()
                .Where(x => x.BusinessCustomId == businessId).FirstOrDefaultAsync();
            var result = mapper.Map<BusinessEntity, OrganizationResponse>(query);
            result.OrgAddress = (string.IsNullOrEmpty(query.Building) ? "" : "อาคาร " + (query.Building + " ")) +
                                (string.IsNullOrEmpty(query.RoomNo) ? "" : "ห้อง " + (query.RoomNo + " ")) +
                                (string.IsNullOrEmpty(query.Floor) ? "" : "ชั้น " + (query.Floor + " ")) +
                                (string.IsNullOrEmpty(query.Village) ? "" : "หมู่บ้่าน " + (query.Village + " ")) +
                                (string.IsNullOrEmpty(query.No) ? "" : "เลขที่ " + (query.No + " ")) +
                                (string.IsNullOrEmpty(query.Moo) ? "" : "หมู่ " + (query.Moo + " ")) +
                                (string.IsNullOrEmpty(query.Alley) ? "" : "ซอย " + (query.Alley + " ")) +
                                (string.IsNullOrEmpty(query.Road) ? "" : "ถนน " + (query.Road + " ")) +
                                (string.IsNullOrEmpty(query.SubDistrict)
                                    ? ""
                                    : "แขวง " + (systemRepository.GetAll<SubDistrictEntity>()
                                        .Where(x => x.SubDistrictCode.ToString().Equals(query.SubDistrict))
                                        .FirstOrDefault().SubDistrictNameTh + " ")) +
                                (string.IsNullOrEmpty(query.District)
                                    ? ""
                                    : "เขต " + (systemRepository.GetAll<DistrictEntity>()
                                        .Where(x => x.DistrictCode.ToString().Equals(query.District)).FirstOrDefault()
                                        .DistrictNameTh + " ")) +
                                (string.IsNullOrEmpty(query.Province)
                                    ? ""
                                    : "จังหวัด " + (systemRepository.GetAll<ProvinceEntity>()
                                        .Where(x => x.ProvinceCode.ToString().Equals(query.Province)).FirstOrDefault()
                                        .ProvinceNameTh + " ")) +
                                (string.IsNullOrEmpty(query.PostCode) ? "" : "รหัสไปรษณีย์ " + query.PostCode);
            result.Url = query.Url;
            result.HotLine = query.Hotline;
            result.Tel = query.Tel;
            result.Email = query.Email;

            return result;
        }

        public async Task<List<OrganizationUserResponse>> GetUserAllowedOrganization(string userName)
        {
            var query = await organizationRepository!.GetUserAllowedOrganizationAsync(userName);
            return mapper.Map<IEnumerable<OrganizationUserEntity>, List<OrganizationUserResponse>>(query);
        }

        public async Task<List<OrganizationUserResponse>> GetUserAllUser(string orgId)
        {
            organizationRepository!.SetCustomOrgId(orgId);
            var query = await organizationRepository!.GetUserListAsync().ToListAsync();
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
            if (await userService.GetUserByName(orgId, userName) == null ||
                await organizationRepository!.GetUserInOrganization(userName) == null)
                throw new KeyNotFoundException("1102");
            return true;
        }
    }
}