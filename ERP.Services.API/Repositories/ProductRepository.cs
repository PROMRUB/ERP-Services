using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

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

        public IQueryable<ProductEntity> GetProductList(string keyword)
        {
            keyword = keyword.ToLower();
            return context.Products.Where(x =>
                x.ProductName.ToLower().Contains(keyword) || x.ProductCustomId.Contains(keyword));
        }

        public IQueryable<ProductEntity> GetProductByCustomId(string keyword)
        {
            return context.Products.Where(x => x.ProductCustomId.Equals(keyword));
        }

        public IQueryable<ProductEntity> GetProductListQueryable()
        {
            return context.Products;
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

        public void AddProducts(List<ProductEntity> products)
        {
            if (products == null || !products.Any())
            {
                throw new ArgumentException("Product list is empty or null.", nameof(products));
            }

            try
            {
                context.Products.AddRange(products); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding products: {ex.Message}");
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
