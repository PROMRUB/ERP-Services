using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.ResponseModels.Customer;
using ERP.Services.API.Repositories;
using System.Text.RegularExpressions;

namespace ERP.Services.API.Interfaces
{
    public interface ICustomerService
    {
        public Task<List<CustomerResponse>> GetCustomerByBusinessAsync(string orgId, Guid businessId);
        public Task<CustomerResponse> GetCustomerInformationByIdAsync(string orgId, Guid businessId, Guid customerId);
        public Task CreateCustomer(string orgId, CustomerRequest request);
        public Task ImportExcel(string orgId, Guid businessId, IFormFile request);
        public Task UpdateCustomer(string orgId, Guid businessId, Guid customerId, CustomerRequest request);
        public Task DeleteCustomer(string orgId, List<CustomerRequest> request);
    }
}
