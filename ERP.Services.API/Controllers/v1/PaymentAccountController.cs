using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
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
    }
}
