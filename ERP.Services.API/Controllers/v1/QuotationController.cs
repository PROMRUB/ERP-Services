using ERP.Services.API.Entities;
using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.Customer;
using ERP.Services.API.Models.ResponseModels.Product;
using ERP.Services.API.Models.ResponseModels.Quotation;
using ERP.Services.API.Repositories;
using ERP.Services.API.Services.Customer;
using Microsoft.AspNetCore.Http;
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
    }
}