﻿using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.ApiKey;
using ERP.Services.API.Models.ResponseModels.ApiKey;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class ApiKeyController : BaseController
    {
        private readonly IApiKeyService services;
        public ApiKeyController(IApiKeyService services)
        {
            this.services = services;
        }

        [HttpPost]
        [Route("org/{id}/action/GetApiKeys")]
        [MapToApiVersion("1")]
        public IActionResult GetApiKeys(string id)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = services.GetApiKeys(id);
                return Ok(ResponseHandler.Response<List<ApiKeyResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/VerifyApiKey/{apiKey}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> VerifyApiKey(string id, string apiKey)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await services.VerifyApiKey(id, apiKey);
                return Ok(ResponseHandler.Response<ApiKeyResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/AddApiKey")]
        [MapToApiVersion("1")]
        public IActionResult AddApiKey(string id, [FromBody] ApiKeyRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.AddApiKey(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/UpdateApiKey/{key}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateApiKey(string id, [FromBody] ApiKeyRequest request, Guid key)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id) || key == Guid.Empty)
                    throw new ArgumentException("1101");
                await services.Update(id, request, key);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpDelete]
        [Route("org/{id}/action/DeleteApiKeyById/{keyId}")]
        [MapToApiVersion("1")]
        public IActionResult DeleteApiKeyById(string id, string keyId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                services.DeleteApiKeyById(id, keyId);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
