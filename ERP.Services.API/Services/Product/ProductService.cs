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
        private readonly IQuotationService quotationService;
        public ProductService(IMapper mapper,
            IProductRepository productRepository,
            IOrganizationRepository organizationRepository,
            IQuotationService quotationService)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.organizationRepository = organizationRepository;
            this.quotationService = quotationService;
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

            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var itemsToInsert = new List<ProductEntity>();
            var itemsToUpdate = new List<ProductEntity>();
            int updatedCount = 0;

            try
            {
                await using (var stream = new MemoryStream())
                {
                    await request.CopyToAsync(stream);
                    stream.Position = 0;

                    using (var package = new OfficeOpenXml.ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension?.Rows ?? 0;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            string customId = worksheet.Cells[row, 1]?.Text.Trim() ?? string.Empty;
                            if (string.IsNullOrEmpty(customId)) continue;

                            string productName = worksheet.Cells[row, 2]?.Text.Trim() ?? string.Empty;

                            // parse แบบ culture-invariant กันจุด/จุลภาค
                            string s3  = worksheet.Cells[row, 3]?.Text;
                            string s4  = worksheet.Cells[row, 4]?.Text;
                            string s6  = worksheet.Cells[row, 6]?.Text;
                            string s8  = worksheet.Cells[row, 8]?.Text;
                            string s10 = worksheet.Cells[row,10]?.Text;
                            string s11 = worksheet.Cells[row,11]?.Text;
                            string s12 = worksheet.Cells[row,12]?.Text;
                            string s14 = worksheet.Cells[row,14]?.Text;
                            string s15 = worksheet.Cells[row,15]?.Text;

                            decimal.TryParse(s3,  System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal msrp);
                            decimal.TryParse(s4,  System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal lwPrice);
                            decimal.TryParse(s6,  System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal costInhand);
                            decimal.TryParse(s8,  System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal costLastPO);
                            decimal.TryParse(s10, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal buyunitEst);
                            decimal.TryParse(s11, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal whtEst);
                            decimal.TryParse(s12, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal exchangeRateEst);
                            decimal.TryParse(s14, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal importDutyEst);
                            decimal.TryParse(s15, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal administrativeCostEst);

                            string currencyInhand = worksheet.Cells[row, 5]?.Text.Trim()  ?? string.Empty;
                            string currencyLastPO = worksheet.Cells[row, 7]?.Text.Trim()  ?? string.Empty;
                            string currencyEst    = worksheet.Cells[row, 9]?.Text.Trim()  ?? string.Empty;
                            string incotermEst    = worksheet.Cells[row,13]?.Text.Trim()  ?? string.Empty;

                            var product = await productRepository
                                .GetProductByCustomId((Guid)organization.OrgId!, businessId, customId)
                                .FirstOrDefaultAsync();

                            if (product == null)
                            {
                                var newProduct = new ProductEntity
                                {
                                    ProductId              = Guid.NewGuid(),
                                    OrgId                  = organization.OrgId,
                                    BusinessId             = businessId,
                                    ProductCatId           = Guid.Empty,
                                    ProductSubCatId        = Guid.Empty,
                                    ProductCustomId        = customId,
                                    ProductName            = productName,
                                    MSRP                   = msrp,
                                    LwPrice                = lwPrice,
                                    CurrencyInhand         = currencyInhand,
                                    CostInhand             = costInhand,
                                    CurrencyLastPO         = currencyLastPO,
                                    CostLastPO             = costLastPO,
                                    CurrencyEst            = currencyEst,
                                    BuyUnitEst             = buyunitEst,
                                    WHTEst                 = whtEst,
                                    ExchangeRateEst        = exchangeRateEst,
                                    IncortermEst           = incotermEst,
                                    ImportDutyEst          = importDutyEst,
                                    AdministrativeCostEst  = administrativeCostEst,
                                    ProductStatus          = RecordStatus.Active.ToString()
                                };

                                itemsToInsert.Add(newProduct);
                            }
                            else
                            {
                                product.ProductName            = productName;
                                product.MSRP                   = msrp;
                                product.LwPrice                = lwPrice;
                                product.CurrencyInhand         = currencyInhand;
                                product.CostInhand             = costInhand;
                                product.CurrencyLastPO         = currencyLastPO;
                                product.CostLastPO             = costLastPO;
                                product.CurrencyEst            = currencyEst;
                                product.BuyUnitEst             = buyunitEst;
                                product.WHTEst                 = whtEst;
                                product.ExchangeRateEst        = exchangeRateEst;
                                product.IncortermEst           = incotermEst;
                                product.ImportDutyEst          = importDutyEst;
                                product.AdministrativeCostEst  = administrativeCostEst;

                                productRepository.UpdateProduct(product);
                                itemsToUpdate.Add(product);
                                updatedCount++;
                            }
                        }
                    }
                }

                if (itemsToInsert.Count > 0)
                    productRepository.AddProducts(itemsToInsert);

                productRepository.Commit(); // ให้ทุก product เซฟให้เสร็จก่อน

                // ✅ รอให้ทุก ImportUpdate เสร็จใน request scope เดียวกัน (กัน DbContext ถูก Dispose)
                var updateTasks = new List<Task>(itemsToInsert.Count + itemsToUpdate.Count);
                foreach (var item in itemsToInsert)
                    updateTasks.Add(quotationService.ImportUpdate((Guid)item.ProductId));
                foreach (var item in itemsToUpdate)
                    updateTasks.Add(quotationService.ImportUpdate((Guid)item.ProductId));

                await Task.WhenAll(updateTasks);

                Console.WriteLine($"Inserted: {itemsToInsert.Count}, Updated: {updatedCount}");
            }
            catch
            {
                // log แล้วค่อยโยนต่อถ้าต้องการ
                throw;
            }
        }
    }
}