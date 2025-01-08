using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Authorization;
using ERP.Services.API.Models.RequestModels.User;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class AuthorizationController : BaseController
    {
        private readonly IUserService userService;
        public AuthorizationController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("org/{id}/action/SignIn")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SignIn(string id, [FromBody] SignInRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                    throw new ArgumentException("1101");
                var result = await userService.SignIn(id, request);
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetUserProfile")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await userService.GetUserProfile();
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }


        [HttpPost]
        [Route("org/{id}/action/AddUserToBusiness")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AddUserToBusiness(string id, AddUserToBusinessRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await userService.AddUserToBusinessAsync(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost   ]
        [Route("org/{id}/action/AddRole/{userId}/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> AddUserRole(string id, Guid userId, Guid businessId, AddUserToBusinessRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await userService.AddRoleToUser(id, userId, businessId, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        
        [HttpGet]
        [Route("org/{id}/action/GetUserRole/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetUserRole(string id,Guid businessId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await userService.GetRole(id, businessId);
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/SignOut")]
        [MapToApiVersion("1")]
        public IActionResult SignOut(string id, [FromBody] SignOutRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(request.Username))
                    throw new ArgumentException("1101");
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
