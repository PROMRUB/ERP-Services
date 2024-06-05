using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.ResponseModels.Customer;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.ComponentModel;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ERP.Services.API.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper mapper;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly ISystemConfigRepository systemConfigRepository;
        public CustomerService(IMapper mapper,
            ICustomerRepository customerRepository,
            IOrganizationRepository organizationRepository,
            ISystemConfigRepository systemConfigRepository)
        {
            this.mapper = mapper;
            this.customerRepository = customerRepository;
            this.organizationRepository = organizationRepository;
            this.systemConfigRepository = systemConfigRepository;
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
            query.CusCustomId = "C." + runNo.Character + "-" + runNo.Allocated.Value.ToString("D5") + ".D";
            customerRepository.CreateCustomer(query);
            customerRepository.Commit();
        }

        public async Task ImportExcel(string orgId, Guid businessId, IFormFile request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var items = new List<CustomerEntity>();

            var subDistricts = await systemConfigRepository.GetSubDistrictList().ToListAsync();
            var districts = await systemConfigRepository.GetDistrictList().ToListAsync();
            var provinces = await systemConfigRepository.GetProvinceList().ToListAsync();
            using (var stream = new MemoryStream())
            {
                request.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        items.Add(new CustomerEntity
                        {
                            BusinessId = businessId,
                            CusType = (worksheet.Cells[row, 2].Text).Contains("น") ? BusinessType.Corporate.ToString() : BusinessType.Individual.ToString(),
                            TaxId = worksheet.Cells[row, 3].Text,
                            BrnId = worksheet.Cells[row, 4].Text,
                            CusNameEng = worksheet.Cells[row, 5].Text,
                            CusName = worksheet.Cells[row, 6].Text,
                            DisplayName = worksheet.Cells[row, 7].Text,
                            Website = worksheet.Cells[row, 8].Text,
                            Building = worksheet.Cells[row, 9].Text,
                            RoomNo = worksheet.Cells[row, 11].Text,
                            Floor = worksheet.Cells[row, 12].Text,
                            Village = worksheet.Cells[row, 13].Text,
                            No = worksheet.Cells[row, 14].Text,
                            Moo = worksheet.Cells[row, 15].Text,
                            Alley = worksheet.Cells[row, 16].Text,
                            Road = worksheet.Cells[row, 17].Text,
                            SubDistrict = worksheet.Cells[row, 18].Text.Replace("แขวง", "").Replace("ตำบล", ""),
                            District = worksheet.Cells[row, 19].Text.Replace("เขต", "").Replace("อำเภอ", ""),
                            Province = worksheet.Cells[row, 20].Text.Replace("จังหวัด", ""),
                            PostCode = worksheet.Cells[row, 21].Text,
                            OrgId = organization.OrgId,
                            CusStatus = RecordStatus.Active.ToString(),
                            CusCreatedDate = DateTime.UtcNow
                        });
                    }
                }
            }

            foreach (var item in items)
            {
                string cleanedCusNameEng = Regex.Replace(item.CusNameEng, "[^a-zA-Z0-9]+", "");
                var customer = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId).Where(x => x.TaxId.Equals(item.TaxId) && x.BrnId.Equals(item.BrnId)).FirstOrDefaultAsync();
                if (customer == null)
                {
                    char firstCharacter = cleanedCusNameEng.ToUpper().FirstOrDefault();
                    var runNo = await customerRepository.CustomerNumberAsync((Guid)organization.OrgId, (Guid)businessId, firstCharacter.ToString());
                    item.CusCustomId = "C." + runNo.Character + "-" + runNo.Allocated.Value.ToString("D5") + ".D";
                    item.SubDistrict = string.IsNullOrEmpty(item.SubDistrict) ? "" : subDistricts.FirstOrDefault(p => p.SubDistrictNameTh.ToUpper().Contains(item.SubDistrict.ToUpper()) || p.SubDistrictNameEn.ToUpper().Contains(item.SubDistrict.ToUpper()))?.SubDistrictCode.ToString() ?? "";
                    item.District = string.IsNullOrEmpty(item.District) ? "" : districts.FirstOrDefault(p => p.DistrictNameTh.ToUpper().Contains(item.District.ToUpper()) || p.DistrictNameEn.ToUpper().Contains(item.District.ToUpper()))?.DistrictCode.ToString() ?? "";
                    item.Province = string.IsNullOrEmpty(item.Province) ? "" : provinces.FirstOrDefault(p => p.ProvinceNameTh.ToUpper().Contains(item.Province.ToUpper()) || p.ProvinceNameEn.ToUpper().Contains(item.Province.ToUpper()))?.ProvinceCode.ToString() ?? "";
                    customerRepository.CreateCustomer(item);
                }
            }
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
