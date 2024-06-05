using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Organization;
using ERP.Services.API.Models.ResponseModels.Organization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationService services;

        public OrganizationController(IOrganizationService services)
        {
            this.services = services;
        }

        [HttpGet]
        [Route("org/{id}/action/GetOrganization")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetOrganization(string id)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GetOrganization(id);
                return Ok(ResponseHandler.Response<OrganizationResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetOrganization/{taxId}/{brnId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetOrganizationByTaxId(string id, string taxId, string brnId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GetOrganizationByTaxId(id, taxId, brnId);
                return Ok(ResponseHandler.Response<OrganizationResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/AdminGetUserAllowedOrganization/{userName}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AdminGetUserAllowedOrganization(string id, string userName)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GetUserAllowedOrganization(userName!);
                return Ok(ResponseHandler.Response<List<OrganizationUserResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/AddUserToOrganization")]
        [MapToApiVersion("1")]
        public IActionResult AddUserToOrganization(string id, [FromBody] OrganizationUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.AddUserToOrganization(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/AdminAddOrganization")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AdminAddOrganization(string id, [FromBody] OrganizationRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await services.AddOrganization(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/AdminAddBusiness")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AdminAddBusiness(string id, [FromBody] OrganizationRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await services.AddBusiness(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetBusiness")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetBusiness(string id)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GetBusiness(id);
                return Ok(ResponseHandler.Response<List<OrganizationResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetBusiness/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetBusiness(string id,string businessId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.GetBusinessById(id, businessId);
                return Ok(ResponseHandler.Response<OrganizationResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/AdminUpdateOrganization")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AdminUpdateOrganization(string id, [FromBody] OrganizationRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await services.UpdateOrganization(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/AdminAddUserToOrganization")]
        [MapToApiVersion("1")]
        public IActionResult AdminAddUserToOrganization(string id, [FromBody] OrganizationUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var userOrgId = request.OrgCustomId;
                services.AddUserToOrganization(userOrgId!, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
