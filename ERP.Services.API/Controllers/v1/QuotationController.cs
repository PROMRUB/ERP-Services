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
    public class QuotationController(IQuotationService service) : BaseController
    {
        public record QuotationKeyword(
            string? Keyword,
            Guid BusinessId,
            string? StartDate,
            string? EndDate,
            Guid? CustomerId,
            Guid? ProjectId,
            int? Profit,
            bool? IsSpecialPrice,
            Guid? SalesPersonId,
            string? Status,
            int Page = 1,
            int PageSize = 10,
            bool? IsGreaterThan = null);

        public record UpdateProductQuotationParameter(Guid QuotationId, Guid ProductId, double EstimateCost,double Cost);

        private readonly IQuotationService _service = service;

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetByKeyword([FromQuery] QuotationKeyword resource)
        {
            try
            {
                var result = await _service.GetByList(resource.Keyword ?? null, resource.BusinessId, resource.StartDate,
                    resource.EndDate, resource.CustomerId, resource.ProjectId, resource.Profit, resource.IsSpecialPrice,
                    resource.SalesPersonId, resource.Status, resource.Page,
                    resource.PageSize, resource.IsGreaterThan);
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet("{id:guid}/total_product")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetTotalProductQuotation(Guid id)
        {
            try
            {
                var result = await _service.GetTotalProductQuotation(id);
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost("update_product_quotation")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateProductQuotation([FromBody] UpdateProductQuotationParameter parammeter)
        {
            try
            {
                var result = await _service.UpdateCostEstimateQuotation(parammeter.QuotationId, parammeter.ProductId,
                    parammeter.EstimateCost,parammeter.Cost);
                return Ok(ResponseHandler.Response("1000", null, result));
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
                var result = await _service.QuotationStatus();
                return Ok(ResponseHandler.Response("1000", null, result));
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
                var result = await _service.Create(resource);
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response("message : " + ex.Message,
                    "inner Exception: " + ex.InnerException?.Message));
            }
        }

        [HttpPost("{id:guid}/update_status")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] QuotationResource resource)
        {
            try
            {
                var result = await _service.UpdateStatus(id, resource);
                return Ok(ResponseHandler.Response("1000", null, result));
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
                var result = await _service.Update(id, resource);
                return Ok(ResponseHandler.Response("1000", null, result));
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
                var result = await _service.Calculate(resource);
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
                await _service.Delete(id);
                return Ok(ResponseHandler.Response<QuotationResponse>("1000", null, null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpDelete("all")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                await _service.DeleteAll();
                return Ok(ResponseHandler.Response<QuotationResponse>("1000", null, null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        public record PaymentAccountList(string OrgId, Guid BusinessId);

        public record PaymentAccountById(string OrgId, Guid BusinessId, Guid PaymentAccountId);

        public record BaseParameter(string? Keyword, Guid BusinessId, int Page, int PageSize);

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
        public async Task<IActionResult> Get([FromQuery] QuotationKeyword resource)
        {
            try
            {
                var result = await service.GetByList(resource.Keyword ?? null, resource.BusinessId, resource.StartDate,
                    resource.EndDate, resource.CustomerId, resource.ProjectId, resource.Profit, resource.IsSpecialPrice,
                    resource.SalesPersonId, resource.Status, resource.Page,
                    resource.PageSize, resource.IsGreaterThan);
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
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        public record ApproveStatus(string Status);

        [HttpPost("approve_sale_price/{id:guid}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> ApproveSalePrice(Guid id)
        {
            try
            {
                var result = await service.ApproveSalePrice(id);

                return Ok(ResponseHandler.Response("1000", null, result));
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
                var result = await service.ApproveQuotation(id);
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("pdf/{id:guid}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Pdf(Guid id)
        {
            try
            {
                QuotationDocument document = await service.GeneratePDF(id);

                byte[] pdfBytes = document.GeneratePdf();
                MemoryStream ms = new MemoryStream(pdfBytes);
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = document.FileName + ".pdf",
                };
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message + "/// " + ex.InnerException?.Message, null));
            }
        }
    }
}
