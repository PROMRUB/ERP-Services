using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;

namespace ERP.Services.API.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(PromDbContext context) {
            this.context = context;
        }

        public IQueryable<ProductCategoryEntity> GetProductCategoryByBusiness(Guid orgId, Guid businessId)
        {
            return context.ProductCategories.Where(x => x.OrgId == orgId && x.BusinessId == businessId);
        }
        public IQueryable<ProductEntity> GetProductByBusiness(Guid orgId, Guid businessId)
        {
            return context.Products.Where(x => x.OrgId == orgId && x.BusinessId == businessId);
        }

        public void AddProductCategory(ProductCategoryEntity query) {
            try
            {
                context.ProductCategories.Add(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddProduct(ProductEntity query)
        {
            try
            {
                context.Products.Add(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateProductCategory(ProductCategoryEntity query)
        {
            try
            {
                context.ProductCategories.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateProduct(ProductEntity query)
        {
            try
            {
                context.Products.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteProductCategory(ProductCategoryEntity query)
        {
            try
            {
                query.CategoryStatus = RecordStatus.InActive.ToString();
                context.ProductCategories.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteProduct(ProductEntity query)
        {
            try
            {
                query.ProductStatus = RecordStatus.InActive.ToString();
                context.Products.Update(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
