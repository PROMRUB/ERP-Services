using AutoMapper;
using ERP.Services.API.Enum;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.ResponseModels.Product;
using ERP.Services.API.Repositories;
using OfficeOpenXml;

namespace ERP.Services.API.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductCategoryResponse>> GetProducCategorytListByBusiness(string orgId, Guid businessId);
        public Task<List<ProductResponse>> GetProductListByBusiness(string orgId, Guid businessId,string keyword);
        public Task<ProductResponse> GetProductInformationById(string orgId, Guid businessId, Guid productId);
        public Task CreateProduct(string orgId, ProductRequest request);
        public Task CreateProductCategory(string orgId, ProductCategoryRequest request);
        public Task UpdateProduct(string orgId, Guid businessId, Guid productId, ProductRequest request);
        public Task UpdateProductCategory(string orgId, Guid businessId, Guid productCatId, ProductCategoryRequest request);
        public Task DeleteProduct(string orgId, Guid businessId, Guid productId, ProductRequest request);
        public Task DeleteProductCategory(string orgId, Guid businessId, Guid productCatId, ProductCategoryRequest request);
        public Task ImportProductCategory(string orgId, Guid businessId, IFormFile request);
        public Task ImportProduct(string orgId, Guid businessId, IFormFile request);
    }
}
