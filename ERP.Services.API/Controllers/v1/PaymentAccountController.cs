using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Condition;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Services.Condition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class PaymentAccountController : BaseController
    {
        private readonly IPaymentAccountService paymentAccountService;

        public PaymentAccountController(IPaymentAccountService paymentAccountService)
        {
            this.paymentAccountService = paymentAccountService;
        }

        [HttpGet]
        [Route("org/{id}/action/GetPaymentAccount/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetPaymentAccount(string id, Guid businessId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await paymentAccountService.GetPaymentAccountListByBusiness(id, businessId);
                return Ok(ResponseHandler.Response<List<PaymentAccountResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        
        public record PaymentAccountResourceParameter(string? Keyword,int Page,int PageSize);
        
        [HttpGet]
        [Route("org/{id}/action/GetPaymentAccountWithPaging/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetPaymentAccountListWithPaging(string id, Guid businessId
            ,[FromQuery] PaymentAccountResourceParameter resourceParameter)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await paymentAccountService.GetPaymentAccountListByBusiness(id, businessId,
                    resourceParameter.Keyword,resourceParameter.Page,resourceParameter.PageSize);
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }


        [HttpPost]
        [Route("org/{id}/action/CreatePaymentAccount")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreatePaymentAccount(string id, PaymentAccountRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await paymentAccountService.CreatePaymentAccount(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
