using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Customer;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.ResponseModels.Customer;
using ERP.Services.API.Models.ResponseModels.Product;
using ERP.Services.API.Repositories;
using ERP.Services.API.Services.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class ProductController : BaseController
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [Route("org/{id}/action/GetProductCategoryList/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetProductCategoryList(string id, Guid businessId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await productService.GetProducCategorytListByBusiness(id, businessId);
                return Ok(ResponseHandler.Response<List<ProductCategoryResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetProductList/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetProductList(string id, Guid businessId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await productService.GetProductListByBusiness(id, businessId);
                return Ok(ResponseHandler.Response<List<ProductResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetProductInformation/{businessId}/{productId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetProducInformation(string id, Guid businessId, Guid productId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await productService.GetProductInformationById(id, businessId, productId);
                return Ok(ResponseHandler.Response<ProductResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        [HttpPost]
        [Route("org/{id}/action/CreateProductCategory")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateProductCategory(string id, [FromBody] ProductCategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await productService.CreateProductCategory(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/CreateProduct")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateProduct(string id, [FromBody] ProductRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await productService.CreateProduct(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        [HttpPost]
        [Route("org/{id}/action/UpdateProductCategory/{businessId}/{productCatId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateProductCategory(string id, Guid businessId, Guid productCatId, [FromBody] ProductCategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await productService.UpdateProductCategory(id, businessId, productCatId, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/UpdateProduct/{businessId}/{productId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateProduct(string id, Guid businessId, Guid productId, [FromBody] ProductRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await productService.UpdateProduct(id, businessId, productId, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/DeleteProductCategory/{businessId}/{productCatId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteProductCategory(string id, Guid businessId, Guid productCatId, [FromBody] ProductCategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await productService.DeleteProductCategory(id, businessId, productCatId, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/DeleteProduct/{businessId}/{productId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteProduct(string id, Guid businessId, Guid productId, [FromBody] ProductRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await productService.DeleteProduct(id, businessId, productId, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
