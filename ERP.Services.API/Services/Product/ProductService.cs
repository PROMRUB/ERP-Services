using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Product;
using ERP.Services.API.Models.ResponseModels.Customer;
using ERP.Services.API.Models.ResponseModels.Product;
using ERP.Services.API.Repositories;
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
            var result = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId).Where(x => x.CategoryStatus == RecordStatus.Active.ToString()).OrderBy(x => x.CustomCatId).ToListAsync();
            return mapper.Map<List<ProductCategoryEntity>, List<ProductCategoryResponse>>(result);
        }

        public async Task<List<ProductResponse>> GetProductListByBusiness(string orgId, Guid businessId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await productRepository.GetProductByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProductStatus == RecordStatus.Active.ToString()).OrderBy(x => x.ProductCustomId).ToListAsync();
            var result = mapper.Map<List<ProductEntity>, List<ProductResponse>>(query);
            var cat = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId).ToListAsync();
            foreach (var item in result)
            {
                item.ProductCategory = cat.Where(x => x.ProductCatId == item.ProductCatId).FirstOrDefault()?.CategoryName;
                item.ProductSubCategory = cat.Where(x => x.ProductCatId == item.ProductSubCatId).FirstOrDefault()?.CategoryName;
                if (item.ProductStatus == RecordStatus.Active.ToString())
                {
                    item.ProductStatus = "ปกติ";
                }
            }
            return result;
        }

        public async Task<ProductResponse> GetProductInformationById(string orgId, Guid businessId, Guid productId)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var result = await productRepository.GetProductByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProductId == productId).FirstOrDefaultAsync();
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
            var query = await productRepository.GetProductByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            query.ProductName = request.ProductName;
            query.MSRP = request.MSRP;
            query.LwPrice = request.LwPrice;
            productRepository.UpdateProduct(query);
            productRepository.Commit();
        }

        public async Task UpdateProductCategory(string orgId, Guid businessId, Guid productCatId, ProductCategoryRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProductCatId == productCatId).FirstOrDefaultAsync();
            query.ParentCatId = request.ParentCatId;
            query.CategoryName = request.CategoryName;
            productRepository.UpdateProductCategory(query);
            productRepository.Commit();
        }
        public async Task DeleteProduct(string orgId, Guid businessId, Guid productId, ProductRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await productRepository.GetProductByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            productRepository.DeleteProduct(query);
            productRepository.Commit();
        }

        public async Task DeleteProductCategory(string orgId, Guid businessId, Guid productCatId, ProductCategoryRequest request)
        {
            organizationRepository.SetCustomOrgId(orgId);
            var organization = await organizationRepository.GetOrganization();
            var query = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId).Where(x => x.ProductCatId == productCatId).FirstOrDefaultAsync();
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
                                CategoryStatus = RecordStatus.Active.ToString()
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

            using (var stream = new MemoryStream())
            {
                request.CopyTo(stream);
                stream.Position = 0;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        var category = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId).Where(x => x.CustomCatId.Equals(worksheet.Cells[row, 1].Text)).FirstOrDefaultAsync();
                        var subCategory = await productRepository.GetProductCategoryByBusiness((Guid)organization.OrgId, businessId).Where(x => x.CustomCatId.Equals(worksheet.Cells[row, 2].Text)).FirstOrDefaultAsync();
                        items.Add(new ProductEntity
                        {
                            ProductId = Guid.NewGuid(),
                            OrgId = organization.OrgId,
                            BusinessId = businessId,
                            ProductCatId = category == null ? Guid.Empty : category.ProductCatId,
                            ProductSubCatId = subCategory == null ? Guid.Empty : subCategory.ProductCatId,
                            ProductCustomId = worksheet.Cells[row, 3].Text,
                            ProductName = worksheet.Cells[row, 4].Text,
                            MSRP = decimal.Parse(worksheet.Cells[row, 5].Text),
                            LwPrice = decimal.Parse(worksheet.Cells[row, 6].Text),
                            ProductStatus = RecordStatus.Active.ToString()
                        });
                    }
                    stream.Dispose();
                }
            }

            foreach (var item in items)
            {
                productRepository.AddProduct(item);
            }

            productRepository.Commit();
        }
    }
}
