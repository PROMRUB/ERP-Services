using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.User;
using ERP.Services.API.Models.ResponseModels.Organization;
using ERP.Services.API.Models.ResponseModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class UserController : BaseController
    {
        private readonly IUserService services;
        public UserController(IUserService services)
        {
            this.services = services;
        }

        [HttpPost]
        [Route("org/{id}/action/AdminAddUser")]
        [MapToApiVersion("1")]
        public IActionResult AdminAddUser(string id, [FromBody] UserRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.AddUser(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        
        [HttpPost]
        [Route("org/{id}/action/AdminRunningUser/{businessId:guid}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AdminRunningUser(string id, Guid businessId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await services.RunningUser(id, businessId);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        
        [HttpPost]
        [Route("org/action/ChangeAllPassword")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ChangeAllPassword()
        {
            try
            {
                await services.ChangeAllPassword();
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
