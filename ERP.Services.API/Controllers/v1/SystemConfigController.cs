using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.ResponseModels.ApiKey;
using ERP.Services.API.Models.ResponseModels.SystemConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class SystemConfigController : BaseController
    {
        private readonly ISystemConfigServices systemConfigServices;
        public SystemConfigController(ISystemConfigServices systemConfigServices)
        {
            this.systemConfigServices = systemConfigServices;
        }

        [HttpGet]
        [Route("org/{id}/action/GetProvinceList")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetProvinceList()
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException("1101");
                var result = await systemConfigServices.GetProvincesAsync();
                return Ok(ResponseHandler.Response<List<ProvinceResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetDistrictList")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetDistrictList()
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException("1101");
                var result = await systemConfigServices.GetDistrictsAsync();
                return Ok(ResponseHandler.Response<List<DistrictResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetSubDistrictList")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetSubDistrictList()
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException("1101");
                var result = await systemConfigServices.GetSubDIstrictsAsync();
                return Ok(ResponseHandler.Response<List<SubDIstrictResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
