using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.ResponseModels.Customer;
using ERP.Services.API.Models.ResponseModels.Product;
using ERP.Services.API.Repositories;
using ERP.Services.API.Utils;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ERP.Services.API.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;
        private readonly IOrganizationRepository organizationRepository;

        public ProductService(IMapper mapper,
            IProductRepository productRepository
            , IOrganizationRepository organizationRepository)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.organizationRepository = organizationRepository;
        }

        public async Task<List<ProductCategoryResponse>> GetProducCategorytListByBusiness(string orgId, Guid businessId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.CategoryStatus == RecordStatus.Active.ToString()).OrderBy(x => x.CustomCatId)
                .ToListAsync();
            return mapper.Map<List<ProductCategoryEntity>, List<ProductCategoryResponse>>(result);
        }

        public async Task<List<ProductResponse>> GetProductListByBusiness(string orgId, Guid businessId, string keyword)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await productRepository.GetProductByBusiness((Guid)organization.OrgId, businessId).Where(x =>
                x.ProductStatus == RecordStatus.Active.ToString()
                && (string.IsNullOrWhiteSpace(keyword) ||
                    (!string.IsNullOrWhiteSpace(x.ProductCustomId) && x.ProductCustomId.Contains(keyword))
                    || (!string.IsNullOrWhiteSpace(x.ProductName) && x.ProductName.ToLower().Contains(keyword))
                )
            ).OrderBy(x => x.ProductCustomId).ToListAsync();
            var result = mapper.Map<List<ProductEntity>, List<ProductResponse>>(query);
            var cat = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId)
                .ToListAsync();
            foreach (var item in result)
            {
                item.ProductCategory =
                    cat.Where(x => x.ProductCatId == item.ProductCatId).FirstOrDefault()?.CategoryName;
                item.ProductSubCategory = cat.Where(x => x.ProductCatId == item.ProductSubCatId).FirstOrDefault()
                    ?.CategoryName;
                if (item.ProductStatus == RecordStatus.Active.ToString())
                {
                    item.ProductStatus = "ปกติ";
                }
            }

            return result;
        }

        public async Task<PagedList<ProductResponse>> GetProductListByBusiness(string orgId, Guid businessId,
            string keyword, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
            }
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = productRepository.GetProductByBusiness((Guid)organization.OrgId, businessId)
                .Where(x =>
                x.ProductStatus == RecordStatus.Active.ToString()
                && (string.IsNullOrWhiteSpace(keyword) ||
                    (!string.IsNullOrWhiteSpace(x.ProductCustomId) && x.ProductCustomId.ToLower().Contains(keyword))
                    || (!string.IsNullOrWhiteSpace(x.ProductName) && x.ProductName.ToLower().Contains(keyword))
                )
            ).OrderBy(x => x.ProductCustomId);
            // var result = mapper.Map<List<ProductEntity>, List<ProductResponse>>(query);
            // var cat = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId)
            //     .ToListAsync();
            // foreach (var item in result)
            // {
            //     item.ProductCategory =
            //         cat.Where(x => x.ProductCatId == item.ProductCatId).FirstOrDefault()?.CategoryName;
            //     item.ProductSubCategory = cat.Where(x => x.ProductCatId == item.ProductSubCatId).FirstOrDefault()
            //         ?.CategoryName;
            //     if (item.ProductStatus == RecordStatus.Active.ToString())
            //     {
            //         item.ProductStatus = "ปกติ";
            //     }
            // }

            var result = query.Select(x => new ProductResponse
            {
                ProductId = x.ProductId,
                OrgId = x.OrgId,
                BusinessId = x.BusinessId,
                ProductCatId = x.ProductCatId,
                ProductSubCatId = x.ProductSubCatId,
                // ProductCategory = x.CategoryEntity.CategoryName,
                // ProductSubCategory = x.SubCategoryEntity.CategoryName,
                ProductCustomId = x.ProductCustomId,
                ProductName = x.ProductName,
                MSRP = x.MSRP,
                LwPrice = x.LwPrice,
                ProductStatus = x.ProductStatus == RecordStatus.Active.ToString() ? "ปกติ" : ""
            });

            var paged = await PagedList<ProductResponse>.Create(result, page, pageSize);

            var cat = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId)
                .ToListAsync();
            foreach (var item in paged.Items)
            {
                item.ProductCategory =
                    cat.FirstOrDefault(x => x.ProductCatId == item.ProductCatId)?.CategoryName;
                item.ProductSubCategory = cat.FirstOrDefault(x => x.ProductCatId == item.ProductSubCatId)
                    ?.CategoryName;
            }
            
            
            return paged;
        }

        public async Task<ProductResponse> GetProductInformationById(string orgId, Guid businessId, Guid productId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await productRepository.GetProductByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            return mapper.Map<ProductEntity, ProductResponse>(result);
        }

        public async Task CreateProduct(string orgId, ProductRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = mapper.Map<ProductRequest, ProductEntity>(request);
            request.OrgId = organization.OrgId;
            productRepository.AddProduct(query);
            productRepository.Commit();
        }

        public async Task CreateProductCategory(string orgId, ProductCategoryRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = mapper.Map<ProductCategoryRequest, ProductCategoryEntity>(request);
            query.OrgId = organization.OrgId;
            productRepository.AddProductCategory(query);
            productRepository.Commit();
        }

        public async Task UpdateProduct(string orgId, Guid businessId, Guid productId, ProductRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await productRepository.GetProductByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            query.ProductName = request.ProductName;
            query.MSRP = request.MSRP;
            query.LwPrice = request.LwPrice;
            productRepository.UpdateProduct(query);
            productRepository.Commit();
        }

        public async Task UpdateProductCategory(string orgId, Guid businessId, Guid productCatId,
            ProductCategoryRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.ProductCatId == productCatId).FirstOrDefaultAsync();
            query.ParentCatId = request.ParentCatId;
            query.CategoryName = request.CategoryName;
            productRepository.UpdateProductCategory(query);
            productRepository.Commit();
        }

        public async Task DeleteProduct(string orgId, Guid businessId, Guid productId, ProductRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await productRepository.GetProductByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            productRepository.DeleteProduct(query);
            productRepository.Commit();
        }

        public async Task DeleteProductCategory(string orgId, Guid businessId, Guid productCatId,
            ProductCategoryRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId)
                .Where(x => x.ProductCatId == productCatId).FirstOrDefaultAsync();
            productRepository.DeleteProductCategory(query);
            productRepository.Commit();
        }

        public async Task ImportProductCategory(string orgId, Guid businessId, IFormFile request)
        {
            try
            {
                organizationRepository.SetCustomOrgId(orgId);
                var organization = await organizationRepository.GetOrganization();

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var items = new List<ProductCategoryEntity>();

                using (var stream = new MemoryStream())
                {
                    request.CopyTo(stream);
                    stream.Position = 0;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            items.Add(new ProductCategoryEntity
                            {
                                ProductCatId = Guid.NewGuid(),
                                OrgId = organization.OrgId,
                                BusinessId = businessId,
                                CustomCatId = worksheet.Cells[row, 1].Text,
                                ParentCatId = worksheet.Cells[row, 2].Text,
                                CategoryName = worksheet.Cells[row, 3].Text,
                                CategoryStatus = RecordStatus.InActive.ToString()
                            });
                        }

                        stream.Dispose();
                    }
                }

                foreach (var item in items)
                {
                    productRepository.AddProductCategory(item);
                }

                productRepository.Commit();
            }
            catch
            {
                throw;
            }
        }

        public async Task ImportProduct(string orgId, Guid businessId, IFormFile request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var items = new List<ProductEntity>();

            try
            {
                using (var stream = new MemoryStream())
                {
                    request.CopyTo(stream);
                    stream.Position = 0;

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            string customId = worksheet.Cells[row, 1]?.Text.Trim() ?? string.Empty;
                            if (!string.IsNullOrEmpty(customId))
                            {
                                string productName = worksheet.Cells[row, 2]?.Text ?? string.Empty;

                                decimal msrp = 0;
                                decimal.TryParse(worksheet.Cells[row, 3]?.Text, out msrp);

                                decimal lwPrice = 0;
                                decimal.TryParse(worksheet.Cells[row, 4]?.Text, out lwPrice);

                                var product = await productRepository.GetProductByCustomId((Guid)organization.OrgId!, businessId, customId).FirstOrDefaultAsync();

                                if (product == null)
                                {

                                    var newProduct = new ProductEntity
                                    {
                                        ProductId = Guid.NewGuid(),
                                        OrgId = organization.OrgId,
                                        BusinessId = businessId,
                                        ProductCatId = Guid.Empty,
                                        ProductSubCatId = Guid.Empty,
                                        ProductCustomId = customId,
                                        ProductName = productName,
                                        MSRP = msrp,
                                        LwPrice = lwPrice,
                                        ProductStatus = RecordStatus.Active.ToString()
                                    };

                                    items.Add(newProduct);
                                }
                                else
                                {
                                    product.ProductName = productName;
                                    product.MSRP = msrp;
                                    product.LwPrice = lwPrice;
                                    productRepository.UpdateProduct(product);
                                }
                            }
                        }
                    }
                }
                if(items.Count > 0)
                {
                    productRepository.AddProducts(items);
                }
                productRepository.Commit();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}