using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.ResponseModels.ApiKey;
using ERP.Services.API.Models.ResponseModels.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        [Route("org/{id}/action/GetCustomerList/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCustomerList(string id, Guid businessId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await customerService.GetCustomerByBusinessAsync(id, businessId);
                return Ok(ResponseHandler.Response<List<CustomerResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetCustomerInformation/{businessId}/{customerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetCustomerInformation(string id, Guid businessId, Guid customerId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await customerService.GetCustomerInformationByIdAsync(id, businessId, customerId);
                return Ok(ResponseHandler.Response<CustomerResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/CreateCustomer")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateCustomer(string id, CustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await customerService.CreateCustomer(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost()]
        [Route("org/{id}/action/ImportExcel/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ImportExcel(string id, Guid businessId, IFormFile request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await customerService.ImportExcel(id, businessId, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/UpdateCustomer/{businessId}/{customerId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateCustomer(string id, Guid businessId, Guid customerId, CustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await customerService.UpdateCustomer(id, businessId, customerId, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/DeleteCustomer")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteCustomer(string id, List<CustomerRequest> request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await customerService.DeleteCustomer(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
