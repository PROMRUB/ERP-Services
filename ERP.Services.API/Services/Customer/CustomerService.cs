using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.ResponseModels.Customer;
using ERP.Services.API.Repositories;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using ERP.Services.API.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ERP.Services.API.Services.Customer
{
    public class CustomerService(IMapper mapper,
            ICustomerRepository customerRepository,
            IOrganizationRepository organizationRepository,
            ISystemConfigRepository systemConfigRepository,
            UserPrincipalHandler userPrincipalHandler,
            IPaymentAccountRepository paymentAccountRepositor,
            IUserRepository userRepository,
            IBusinessRepository businessRepository)
        : ICustomerService
    {
        private string selectedItem = string.Empty;

        public async Task<List<CustomerResponse>> GetCustomerByBusinessAsync(string orgId, Guid businessId,
            string keyword)
        {
            keyword = keyword.ToLower();
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.CusStatus != RecordStatus.InActive.ToString()
                            && (string.IsNullOrWhiteSpace(keyword) ||
                                (!string.IsNullOrWhiteSpace(x.CusName) && x.CusName.ToLower().Contains(keyword))
                                || (!string.IsNullOrWhiteSpace(x.CusNameEng) &&
                                    x.CusNameEng.ToLower().Contains(keyword))
                                || (!string.IsNullOrWhiteSpace(x.CusCustomId) &&
                                    x.CusCustomId.ToLower().Contains(keyword))
                            )
                ).OrderBy(x => x.CusCustomId).ToListAsync();
            var result = mapper.Map<List<CustomerEntity>, List<CustomerResponse>>(query);
            foreach (var item in result)
            {
                if (item.CusStatus.Equals(RecordStatus.Waiting.ToString()))
                    item.CusStatus = "รอตรวจสอบ";
                else if (item.CusStatus.Equals(RecordStatus.Active.ToString()))
                    item.CusStatus = "ปกติ";
            }

            return result;
        }

        public async Task<PagedList<CustomerResponse>> GetCustomerByBusinessAsync(string orgId, Guid businessId,
            string keyword, int page, int pageSize)
        {
            keyword = keyword.ToLower();
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var role = await businessRepository.GetUserBusinessList(userPrincipalHandler.Id, (Guid)organization.OrgId!)
                .Where(x => x.BusinessId == businessId).FirstOrDefaultAsync();

            var query = customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.CusStatus != RecordStatus.InActive.ToString()
                            && (string.IsNullOrWhiteSpace(keyword) ||
                                (!string.IsNullOrWhiteSpace(x.DisplayName) && x.DisplayName.ToLower().Contains(keyword))
                                // || (!string.IsNullOrWhiteSpace(x.CusNameEng) &&
                                //     x.CusNameEng.ToLower().Contains(keyword))
                                // || (!string.IsNullOrWhiteSpace(x.CusCustomId) &&
                                //     x.CusCustomId.ToLower().Contains(keyword))
                            )
                ).OrderBy(x => x.CusCustomId);


            if (string.IsNullOrEmpty(keyword) && (role.Role.Contains("Representative") || role.Role.Contains("Admin")))
            {
                query = customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId)
                    .Where(x => false
                    ).OrderBy(x => x.CusCustomId);
            }


            // .ToListAsync();
            // var result = mapper.Map<List<CustomerEntity>, List<CustomerResponse>>(query);
            // foreach (var item in result)
            // {
            //     if (item.CusStatus.Equals(RecordStatus.Waiting.ToString()))
            //         item.CusStatus = "รออนุมัติ";
            //     else if (item.CusStatus.Equals(RecordStatus.Active.ToString()))
            //         item.CusStatus = "ปกติ";
            // }

            var result = query.Select(x => new CustomerResponse
            {
                CusId = x.CusId,
                CusCustomId = x.CusCustomId,
                BusinessId = x.BusinessId,
                CusType = x.CusType,
                CusName = x.CusName,
                CusNameEng = x.CusNameEng,
                DisplayName = x.DisplayName,
                TaxId = x.TaxId,
                BrnId = x.BrnId == "00000" ? "สาขาใหญ่" : x.BrnId,
                Building = x.Building,
                RoomNo = x.RoomNo,
                Floor = x.Floor,
                Village = x.Village,
                Moo = x.Moo,
                No = x.No,
                Road = x.Road,
                Alley = x.Alley,
                Province = x.Province,
                District = x.District,
                SubDistrict = x.SubDistrict,
                PostCode = x.PostCode,
                Website = x.Website,
                IsApprove = x.CusStatus == RecordStatus.Approve.ToString(),
                CusStatus = x.CusStatus.Equals(RecordStatus.Waiting.ToString()) ? "รอตรวจสอบ"
                    : x.CusStatus.Equals(RecordStatus.Active.ToString()) ? "ปกติ"
                    : x.CusStatus.Equals(RecordStatus.Approve.ToString()) ? "ตรวจสอบแล้ว" : ""
            });

            var paged = await PagedList<CustomerResponse>.Create(result, page, pageSize);

            return paged;
        }

        public async Task<CustomerResponse> GetCustomerInformationByIdAsync(string orgId, Guid businessId,
            Guid customerId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.CusId == customerId).FirstOrDefaultAsync();
            var map = mapper.Map<CustomerEntity, CustomerResponse>(result);
            map.FullAddress = result.CusFullAddress;
            map.IsApprove = result.CusStatus == RecordStatus.Approve.ToString();
            return map;
        } 
        
        public async Task<CustomerResponse> GetCustomerInformationWithWordByIdAsync(string orgId, Guid businessId,
            Guid customerId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.CusId == customerId).FirstOrDefaultAsync();
            var map = mapper.Map<CustomerEntity, CustomerResponse>(result);
          
            map.Building = (string.IsNullOrEmpty(result.Building) ? "" : "อาคาร " + (result.Building + " "));
            map.RoomNo = (string.IsNullOrEmpty(result.RoomNo) ? "" : "ห้อง " + (result.RoomNo + " "));
            map.Floor = (string.IsNullOrEmpty(result.Floor) ? "" : "ชั้น " + (result.Floor + " "));
            map.Village = (string.IsNullOrEmpty(result.Village) ? "" : "หมู่บ้่าน " + (result.Village + " "));
            map.No = (string.IsNullOrEmpty(result.No) ? "" : "เลขที่ " + (result.No + " "));
            map.Moo = (string.IsNullOrEmpty(result.Moo) ? "" : "หมู่ " + (result.Moo + " "));
            map.Alley = (string.IsNullOrEmpty(result.Alley) ? "" : "ซอย " + (result.Alley + " "));
            map.Road = (string.IsNullOrEmpty(result.Road) ? "" : "ถนน " + (result.Road + " "));
            map.SubDistrict = string.IsNullOrEmpty(result.SubDistrict) && !result.SubDistrict.Contains("แขวง")
                ? ""
                : "แขวง " + (systemConfigRepository.GetAll<SubDistrictEntity>()
                    .Where(x => x.SubDistrictCode.ToString().Equals(result.SubDistrict)).FirstOrDefault()
                    .SubDistrictNameTh + " ");
            map.District = string.IsNullOrEmpty(result.District) && !result.District.Contains("เขต")
                ? ""
                : "เขต " + (systemConfigRepository.GetAll<DistrictEntity>()
                    .Where(x => x.DistrictCode.ToString().Equals(result.District)).FirstOrDefault()
                    .DistrictNameTh + " ");
            map.Province = (string.IsNullOrEmpty(result.Province) && !result.Province.Contains("จังหวัด")
                ? ""
                : "จังหวัด " + (systemConfigRepository.GetAll<ProvinceEntity>()
                    .Where(x => x.ProvinceCode.ToString().Equals(result.Province)).FirstOrDefault()
                    .ProvinceNameTh + " "));
            map.PostCode = (string.IsNullOrEmpty(result.PostCode) && !result.PostCode.Contains("รหัสไปรษณีย์") ? "" : "รหัสไปรษณีย์ " + result.PostCode);
            map.FullAddress = result.CusFullAddress;

            map.IsApprove = result.CusStatus == RecordStatus.Approve.ToString();
            return map;
        }

        public async Task CreateCustomer(string orgId, CustomerRequest request)
        {
            try
            {
                organizationRepository.SetCustomOrgId(orgId);
                var organization = await organizationRepository.GetOrganization();
                var customer = await customerRepository
                    .GetCustomerByBusiness((Guid)organization.OrgId, (Guid)request.BusinessId).Where(x =>
                        x.CusStatus == RecordStatus.Active.ToString() && x.TaxId.Equals(request.TaxId) &&
                        x.BrnId.Equals(request.BrnId)).OrderBy(x => x.CusCustomId).ToListAsync();
                if (customer.Count != 0)
                {
                    throw new ArgumentException("1111");
                }

                var query = mapper.Map<CustomerRequest, CustomerEntity>(request);
                query.CusFullAddress = request.FullAddress;
                string cleanedCusNameEng = Regex.Replace(request.CusNameEng, "[^a-zA-Z0-9]+", "");
                char firstCharacter = cleanedCusNameEng.ToUpper().FirstOrDefault();
                var runNo = await customerRepository.CustomerNumberAsync((Guid)organization.OrgId,
                    (Guid)request.BusinessId, firstCharacter.ToString(), 1);
                query.OrgId = organization.OrgId;
                query.CusCustomId = "C." + runNo.Character + "-" + runNo.Allocated.Value.ToString("D5") + ".D";
                customerRepository.CreateCustomer(query);
                customerRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ImportExcel(string orgId, Guid businessId, IFormFile request)
        {
            try
            {
                organizationRepository.SetCustomOrgId(orgId);
                var organization = await organizationRepository.GetOrganization();

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var items = new List<CustomerEntity>();

                var subDistricts = await systemConfigRepository.GetAll<SubDistrictEntity>().ToListAsync();
                var districts = await systemConfigRepository.GetAll<DistrictEntity>().ToListAsync();
                var provinces = await systemConfigRepository.GetAll<ProvinceEntity>().ToListAsync();
                using (var stream = new MemoryStream())
                {
                    request.CopyTo(stream);
                    stream.Position = 0;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            items.Add(new CustomerEntity
                            {
                                BusinessId = businessId,
                                CusType = (worksheet.Cells[row, 2].Text).Contains("น")
                                    ? BusinessType.Corporate.ToString()
                                    : BusinessType.Individual.ToString(),
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
                                CusStatus = RecordStatus.Waiting.ToString(),
                                CusCreatedDate = DateTime.UtcNow
                            });
                        }

                        stream.Dispose();
                    }
                }

                foreach (var item in items)
                {
                    string cleanedCusNameEng = Regex.Replace(item.CusNameEng, "[^a-zA-Z0-9]+", "");
                    var customer = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId)
                        .Where(x => x.TaxId.Equals(item.TaxId) && x.BrnId.Equals(item.BrnId)).FirstOrDefaultAsync();
                    if (customer == null && !string.IsNullOrEmpty(cleanedCusNameEng))
                    {
                        selectedItem = item.TaxId;
                        char firstCharacter = cleanedCusNameEng.ToUpper().FirstOrDefault();
                        var runNo = await customerRepository.CustomerNumberAsync((Guid)organization.OrgId,
                            (Guid)businessId, firstCharacter.ToString(), 0);
                        item.CusCustomId = "C." + runNo.Character + "-" + runNo.Allocated.Value.ToString("D5") + ".D";
                        item.SubDistrict = string.IsNullOrEmpty(item.SubDistrict)
                            ? ""
                            : subDistricts.FirstOrDefault(p =>
                                    p.SubDistrictNameTh.ToUpper().Contains(item.SubDistrict.ToUpper()) ||
                                    p.SubDistrictNameEn.ToUpper().Contains(item.SubDistrict.ToUpper()))?.SubDistrictCode
                                .ToString() ?? "";
                        item.District = string.IsNullOrEmpty(item.District)
                            ? ""
                            : districts.FirstOrDefault(p =>
                                    p.DistrictNameTh.ToUpper().Contains(item.District.ToUpper()) ||
                                    p.DistrictNameEn.ToUpper().Contains(item.District.ToUpper()))?.DistrictCode
                                .ToString() ?? "";
                        item.Province = string.IsNullOrEmpty(item.Province)
                            ? ""
                            : provinces.FirstOrDefault(p =>
                                    p.ProvinceNameTh.ToUpper().Contains(item.Province.ToUpper()) ||
                                    p.ProvinceNameEn.ToUpper().Contains(item.Province.ToUpper()))?.ProvinceCode
                                .ToString() ?? "";
                        customerRepository.CreateCustomer(item);
                    }
                }

                customerRepository.Commit();
            }
            catch (Exception)
            {
                throw new Exception(selectedItem);
            }
        }

        public async Task UpdateCustomer(string orgId, Guid businessId, Guid customerId, CustomerRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await customerRepository.GetCustomerByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.CusId == customerId).FirstOrDefaultAsync();
            if (query == null)
                throw new ArgumentException("1111");
            query.CusType = request.CusType;
            query.CusName = request.CusName;
            query.CusNameEng = request.CusNameEng;
            query.DisplayName = request.DisplayName;
            query.TaxId = request.TaxId;
            query.BrnId = request.BrnId;
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
            query.CusFullAddress = request.FullAddress;
            if (request.IsApprove.HasValue && request.IsApprove.Value)
            {
                query.CusStatus = RecordStatus.Approve.ToString();
            }

            customerRepository.UpdateCustomer(query);
            customerRepository.Commit();
        }

        public async Task DeleteCustomer(string orgId, List<CustomerRequest> request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            foreach (var customer in request)
            {
                var query = await customerRepository
                    .GetCustomerByBusiness((Guid)organization.OrgId, (Guid)customer.BusinessId)
                    .Where(x => x.CusId == customer.CusId).FirstOrDefaultAsync();
                customerRepository.DeleteCustomer(query);
            }

            customerRepository.Commit();
        }

        public async Task<List<CustomerContactResponse>> GetCustomerContactByCustomer(string orgId, Guid businessId,
            Guid cusId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            userRepository.SetCustomOrgId(orgId);
            businessRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();

            var user = await userRepository.GetUserProfiles().Where(x => x.OrgUserId == userPrincipalHandler.Id)
                .FirstOrDefaultAsync();
            var role = await businessRepository.GetUserBusinessList(userPrincipalHandler.Id, (Guid)organization.OrgId!)
                .Where(x => x.BusinessId == businessId).FirstOrDefaultAsync();
            List<CustomerContactEntity> result = new List<CustomerContactEntity>();
            if (!role!.Role!.Contains("SaleManager") && !role!.Role!.Contains("Director") && !role!.Role!.Contains("Admin"))
            {
                result = await customerRepository
                    .GetCustomerContactByCustomer((Guid)organization.OrgId, businessId, cusId)
                    .Where(x => x.UserId == userPrincipalHandler.Id && x.CusConStatus == RecordStatus.Active.ToString())
                    .ToListAsync();
            }
            else
            {
                result = await customerRepository
                    .GetCustomerContactByCustomer((Guid)organization.OrgId, businessId, cusId)
                    .Where(x => x.CusConStatus == RecordStatus.Active.ToString()).ToListAsync();
            }

            return mapper.Map<List<CustomerContactEntity>, List<CustomerContactResponse>>(result);
        }

        public async Task<CustomerContactResponse> GetCustomerContactInformationByIdAsync(string orgId, Guid businessId,
            Guid customerId, Guid cusConId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();

            var result = await customerRepository
                .GetCustomerContactByCustomer((Guid)organization.OrgId, businessId, customerId)
                .Where(x => x.UserId == userPrincipalHandler.Id && x.CusConId == cusConId).FirstOrDefaultAsync();
            return mapper.Map<CustomerContactEntity, CustomerContactResponse>(result);
        }

        public async Task CreateCustomerContact(string orgId, Guid businessId, Guid customerId,
            CustomerContactRequest request)
        {
            try
            {
                organizationRepository.SetCustomOrgId(orgId);
                var organization = await organizationRepository.GetOrganization();
                var query = mapper.Map<CustomerContactRequest, CustomerContactEntity>(request);
                query.OrgId = organization.OrgId;
                query.UserId = userPrincipalHandler.Id;
                query.BusinessId = businessId;
                query.CusId = customerId;
                customerRepository.CreateCustomerContact(query);
                customerRepository.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateCustomerContact(string orgId, Guid businessId, Guid customerId, Guid cusConId,
            CustomerContactRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();

            var query = await customerRepository
                .GetCustomerContactByCustomer((Guid)organization.OrgId, businessId, customerId)
                .Where(x => x.UserId == userPrincipalHandler.Id && x.CusConId == cusConId).FirstOrDefaultAsync();
            if (query == null)
                throw new ArgumentException("1111");
            query.CusConFirstname = request.CusConFirstname;
            query.CusConLastname = request.CusConLastname;
            query.TelNo = request.TelNo;
            query.ExtentNo = request.ExtentNo;
            query.MobileNo = request.MobileNo;
            query.Email = request.Email;
            customerRepository.UpdateCustomerContact(query);
            customerRepository.Commit();
        }

        public async Task DeleteCustomerContact(string orgId, List<CustomerContactRequest> request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            foreach (var customer in request)
            {
                var query = await customerRepository
                    .GetCustomerContactByCustomer((Guid)organization.OrgId, (Guid)customer.BusinessId,
                        (Guid)customer.CusId)
                    .Where(x => x.UserId == userPrincipalHandler.Id && x.CusConId == customer.CusConId)
                    .FirstOrDefaultAsync();
                customerRepository.DeleteCustomerContact(query);
            }

            customerRepository.Commit();
        }
    }
}