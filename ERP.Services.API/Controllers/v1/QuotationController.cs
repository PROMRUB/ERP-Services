using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Quotation;
using ERP.Services.API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class QuotationController(IQuotationService service) : BaseController
    {
        public record QuotationKeyword(string Keyword);

        private readonly IQuotationService _service = service;

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetByKeyword([FromQuery] QuotationKeyword keyword)
        {
            try
            {
                var result = await service.GetQuotationById(keyword.Keyword);
                return Ok(ResponseHandler.Response<QuotationResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet("quotation_status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetQuotationStatus()
        {
            try
            {
                var result = await service.QuotationStatus();
                return Ok(ResponseHandler.Response<List<QuotationStatus>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] QuotationResource resource)
        {
            try
            {
                var result = await service.Create(resource);
                return Ok(ResponseHandler.Response<QuotationResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost("{id:guid}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(Guid id, [FromBody] QuotationResource resource)
        {
            try
            {
                var result = await service.Update(id, resource);
                return Ok(ResponseHandler.Response<QuotationResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost("quotation_calculate")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Calculate([FromBody] List<QuotationProductResource> resource)
        {
            try
            {
                var result = await service.Calculate(resource);
                return Ok(ResponseHandler.Response<QuotationResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpDelete("{id:guid}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await service.Delete(id);
                return Ok(ResponseHandler.Response<QuotationResponse>("1000", null, null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        public record PaymentAccountList(string OrgId, Guid BusinessId);

        public record PaymentAccountById(string OrgId, Guid BusinessId, Guid PaymentAccountId);

        public record BaseParameter(string Keyword, Guid BusinessId, int Page, int PageSize);

        [HttpGet("payment_account")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> PaymentAccount([FromQuery] PaymentAccountList query)
        {
            try
            {
                var result = await service.GetPaymentAccountListByBusiness(query.OrgId, query.BusinessId);
                return Ok(ResponseHandler.Response<List<PaymentAccountResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet("payment_account/byid")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> PaymentAccount([FromQuery] PaymentAccountById query)
        {
            try
            {
                var result =
                    await service.GetPaymentAccountInformationById(query.OrgId, query.BusinessId,
                        query.PaymentAccountId);
                return Ok(ResponseHandler.Response<PaymentAccountResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet("list")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Get([FromQuery] BaseParameter query)
        {
            try
            {
                var result = await service.GetByList(query.Keyword, query.BusinessId, query.Page, query.PageSize);
                return Ok(ResponseHandler.Response<PagedList<QuotationResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet("{id:guid}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await service.GetById(id);
                return Ok(ResponseHandler.Response<QuotationResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        
        [HttpPost("approve_sale_price/{id:guid}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ApproveSalePrice(Guid id)
        {
            try
            {
                //TODO:
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        
        [HttpPost("approve_quotation/{id:guid}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ApproveQuotation(Guid id)
        {
            try
            {
                //TODO:
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}