using ERP.Services.API.Entities;
using ERP.Services.API.Enum;

namespace ERP.Services.API.Interfaces
{
    public interface IProductRepository
    {
        public IQueryable<ProductCategoryEntity> GetProductCategoryByBusiness(Guid orgId, Guid businessId);
        public IQueryable<ProductEntity> GetProductByBusiness(Guid businessId);
        public IQueryable<ProductEntity> GetProductByBusiness(Guid orgId, Guid businessId);
        public IQueryable<ProductEntity> GetProductList(string keyword);
        public IQueryable<ProductEntity> GetProductByCustomId(Guid orgId, Guid businessId, string keyword);
        public IQueryable<ProductEntity> GetProductById(Guid productId);
        public IQueryable<ProductEntity> GetProductListQueryable();
        public void AddProductCategory(ProductCategoryEntity query);
        public void AddProduct(ProductEntity query);
        public void AddProducts(List<ProductEntity> products);
        public void UpdateProductCategory(ProductCategoryEntity query);
        public void UpdateProduct(ProductEntity query);
        public void DeleteProductCategory(ProductCategoryEntity query);
        public void DeleteProduct(ProductEntity query);
        public void Commit();
    }
}
