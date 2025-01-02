using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Quotation;
using ERP.Services.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class EmailController(IQuotationService service) : BaseController
    {
        public record EmailRequest(string To, string Name, string Subject, string Body);

        private readonly IQuotationService _service = service;

        [AllowAnonymous]
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SendGeneralMail([FromBody] EmailRequest resource)
        {
            try
            {
                await _service.SendGeneralMail(resource.To, resource.Name, resource.Subject, resource.Body);
                return Ok(ResponseHandler.Response("1000", null, ""));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

    }
}