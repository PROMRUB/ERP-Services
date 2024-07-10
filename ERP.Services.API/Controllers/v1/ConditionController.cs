using ERP.Services.API.Controllers;
using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.ResponseModels.Condition;
using ERP.Services.API.Models.ResponseModels.Customer;
using ERP.Services.API.Services.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class ConditionController : BaseController
    {
        private readonly IConditionService conditionService;

        public ConditionController(IConditionService conditionService)
        {
            this.conditionService = conditionService;
        }

        [HttpGet]
        [Route("org/{id}/action/GetConditonList/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetConditonList(string id, Guid businessId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await conditionService.GetConditionListByBusiness(id, businessId);
                return Ok(ResponseHandler.Response<List<ConditionResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
