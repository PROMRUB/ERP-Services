using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.ResponseModels.Customer;
using ERP.Services.API.Repositories;
using System.Text.RegularExpressions;
using ERP.Services.API.Utils;

namespace ERP.Services.API.Interfaces
{
    public interface ICustomerService
    {
        public Task<List<CustomerResponse>> GetCustomerByBusinessAsync(string orgId, Guid businessId,string keyword);
        public Task<PagedList<CustomerResponse>> GetCustomerByBusinessAsync(string orgId, Guid businessId,string keyword,int page,int pageSize);
        public Task<CustomerResponse> GetCustomerInformationByIdAsync(string orgId, Guid businessId, Guid customerId);
        public Task CreateCustomer(string orgId, CustomerRequest request);
        public Task ImportExcel(string orgId, Guid businessId, IFormFile request);
        public Task UpdateCustomer(string orgId, Guid businessId, Guid customerId, CustomerRequest request);
        public Task DeleteCustomer(string orgId, List<CustomerRequest> request);
        public Task<List<CustomerContactResponse>> GetCustomerContactByCustomer(string orgId, Guid businessId, Guid cusId);
        public Task<CustomerContactResponse> GetCustomerContactInformationByIdAsync(string orgId, Guid businessId, Guid customerId, Guid cusConId);
        public Task CreateCustomerContact(string orgId, Guid businessId, Guid customerId, CustomerContactRequest request);
        public Task UpdateCustomerContact(string orgId, Guid businessId, Guid customerId, Guid cusConId, CustomerContactRequest request);
        public Task DeleteCustomerContact(string orgId, List<CustomerContactRequest> request);
    }
}
