using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Quotation;
using ERP.Services.API.Utils;
using Microsoft.EntityFrameworkCore;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using static ERP.Services.API.Controllers.v1.QuotationController;
using Task = System.Threading.Tasks.Task;

namespace ERP.Services.API.Services.Product;

public class EmailInformation
{
    public string Email { get; set; }
    public string Name { get; set; }
}

public class QuotationService : IQuotationService
{
    private readonly UserPrincipalHandler _userPrincipalHandler;
    private readonly ISystemConfigRepository _systemRepository;
    private readonly IBusinessRepository _businessRepository;
    private readonly IMapper _mapper;
    private readonly IQuotationRepository _quotationRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrganizationRepository _organizationRepository;

    private readonly IPaymentAccountRepository _paymentAccountRepository;

    // public string Email { get; set; } = "kkunayothin@gmail.com";
    // public string Email { get; set; } = "amornrat.t@securesolutionsasia.com";

    private static readonly HashSet<string> ValidIncoterms = new(StringComparer.OrdinalIgnoreCase)
    {
        "N/A", "EXWORK", "FOB", "CIF"
    };

    public List<EmailInformation> Emails { get; set; } = new List<EmailInformation>()
    {
        new EmailInformation()
        {
            Name = "ว\u0e34ชญาดา อภ\u0e34ญ",
            Email = "witchayada.a@securesolutionsasia.com"
        },
        new EmailInformation()
        {
            Name = "kitsada.t@securesolutionsasia.com",
            Email = "kitsada.t@securesolutionsasia.com"
        },
        new EmailInformation()
        {
            Name = "muankhwan.u@securesolutionsasia.com",
            Email = "muankhwan.u@securesolutionsasia.com"
        },
        new EmailInformation()
        {
            Name = "amornrat.t@securesolutionsasia.com",
            Email = "amornrat.t@securesolutionsasia.com"
        },
    };

    // public string Email { get; set; } = "witchayada.a@securesolutionsasia.com";

    // public string Name { get; set; } = "ว\u0e34ชญาดา อภ\u0e34ญ";
    // public string Name { get; set; } = "อมรร\u0e31ตน\u0e4c เท\u0e35ยนบ\u0e38ญยาจารย\u0e4c";

    public QuotationService(IMapper mapper, IQuotationRepository quotationRepository,
        IProductRepository productRepository,
        IOrganizationRepository organizationRepository,
        IPaymentAccountRepository paymentAccountRepository,
        IBusinessRepository businessRepository,
        ISystemConfigRepository systemRepository,
        UserPrincipalHandler userPrincipalHandler)
    {
        try
        {
            _userPrincipalHandler = userPrincipalHandler;
            _mapper = mapper;
            _quotationRepository = quotationRepository;
            _productRepository = productRepository;
            _organizationRepository = organizationRepository;
            _paymentAccountRepository = paymentAccountRepository;
            _businessRepository = businessRepository;
            _systemRepository = systemRepository;
        }
        catch (Exception e)
        {
            Console.WriteLine("=====");
            Console.WriteLine(e.Message);
        }
    }

    public async Task<QuotationResource> GetQuotationById(string keyword)
    {
        var query = _quotationRepository.GetQuotationQuery()
            .Where(x => !string.IsNullOrWhiteSpace(x.QuotationNo) && x.QuotationNo.Contains(keyword));

        var quotation = await query.FirstOrDefaultAsync();

        if (quotation == null)
        {
            return null;
        }

        return await MapEntityToResponse(quotation);
    }

    public async Task<List<QuotationProductResource>> MapProductEntityToResource(List<QuotationProductEntity> entities)
    {
        var list = new List<QuotationProductResource>();
        foreach (var product in entities)
        {
            var selected = await _productRepository.GetProductListQueryable()
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);


            var p = new QuotationProductResource()
            {
                Amount = product.Amount,
                TotalAmount = product.Amount * product.Quantity,
                Unit = "SSA",
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                Discount = Convert.ToInt32(product.Discount),
                Currency = product.Currency,
                BuyUnitEstimate = product.PurchasingPrice,
                ExchangeRate = product.Exchange,
                Incoterm = product.Incoterm,
                CostsEstimate = product.CostEstimate,
                OfferPriceLatest = product.LatestCost,
                CostEstimate = product.CostEstimate,
                CostEstimatePercent = product.CostEstimatePercent,
                TotalCostEstimate = (product.CostEstimate * product.Quantity),
                CostEstimateProfit = product.CostEstimateProfit,
                CostEstimateProfitPercent = product.CostEstimateProfitPercent,
                Order = product.Order,
                IsApproved = (product.Quantity * selected.LwPrice ?? 0) > (decimal)product.Amount,
                AdministrativeCosts = product.AdministrativeCosts,
                ImportDuty = product.ImportDuty,
                WHT = product.WHT
            };

            if (product.Amount != 0)
            {
                p.TotalCostEstimateProfit = product.CostEstimateProfit * product.Quantity;
            }

            list.Add(p);
        }

        return list;
    }

    public List<QuotationProjectResource> MapProjectEntityToResource(List<QuotationProjectEntity> entities)
    {
        return entities.Select(x => new QuotationProjectResource
        {
            ProjectId = x.ProjectId,
            ProjectName = x.Project.ProjectName,
            EthSaleMonth = x.EthSaleMonth?.ToString("MM/yyyy"),
            LeadTime = x.LeadTime,
            Warranty = x.Warranty,
            ConditionId = x.PaymentConditionId,
            PurchaseOrder = x.Po,
        }).ToList();
    }

    public async Task<QuotationResource> MapEntityToResponse(QuotationEntity quotation)
    {
        try
        {
            var response = new QuotationResource
            {
                QuotationId = quotation.QuotationId.Value,
                QuotationNo = quotation.QuotationNo,
                QuotationDateTime = quotation.QuotationDateTime.ToString("dd-MM-yyyy"),
                EditTime = quotation.EditTime,
                CustomerId = quotation.CustomerId,
                ContactPersonId = quotation.CustomerContactId,
                SalesPersonId = quotation.SalePersonId,
                IssuedById = quotation.IssuedById,
                BusinessId = quotation.BusinessId,
                Status = quotation.Status,
                Products = await MapProductEntityToResource(quotation.Products),
                Projects = MapProjectEntityToResource(quotation.Projects),
                Remark = quotation.Remark,
                PaymentAccountId = quotation.PaymentId,
            };

            if (quotation.Projects != null && quotation.Projects.Any())
            {
                response.ProjectName = quotation.Projects.FirstOrDefault()?.Project.ProjectName ?? "";
                response.EthSaleMonth = quotation.Projects.FirstOrDefault()?.EthSaleMonth?.ToString("MM/yyyy");
            }

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<QuotationProductEntity> MutateResourceProduct(List<QuotationProductResource> resources)
    {
        var products = resources.Select((x, i) => new QuotationProductEntity
        {
            Amount = x.Amount,
            ProductId = x.ProductId,
            Discount = x.Discount,
            Quantity = x.Quantity,
            Order = x.Order,
        }).ToList();

        return products;
    }

    public List<QuotationProjectEntity> MutateResourceProject(List<QuotationProjectResource> resources)
    {
        var projects = new List<QuotationProjectEntity>();

        var i = 0;
        foreach (var x in resources)
        {
            var quotationProjectEntity = new QuotationProjectEntity
            {
                ProjectId = x.ProjectId,
                LeadTime = x.LeadTime,
                Warranty = x.Warranty,
                PaymentConditionId = x.ConditionId,
                Po = x.PurchaseOrder,
                Order = ++i
            };

            if (!string.IsNullOrEmpty(x.EthSaleMonth))
            {
                quotationProjectEntity.EthSaleMonth =
                    (DateTime.ParseExact(x.EthSaleMonth, "dd-MM-yyyy", CultureInfo.InvariantCulture));

                quotationProjectEntity.EthSaleMonth =
                    DateTime.SpecifyKind((DateTime)quotationProjectEntity.EthSaleMonth, DateTimeKind.Utc);
            }

            projects.Add(quotationProjectEntity);
        }

        return projects;
    }

    public async Task<QuotationResource> Create(QuotationResource resource)
    {
        try
        {
            var quotation = new QuotationEntity
            {
                EditTime = 0,
                CustomerId = resource.CustomerId,
                CustomerContactId = resource.ContactPersonId,
                QuotationDateTime = DateTime.UtcNow,
                SalePersonId = resource.SalesPersonId,
                IssuedById = resource.IssuedById,
                IsApproved = false,
                Remark = resource.Remark,
                BusinessId = resource.BusinessId,
                Status = resource.Status,
                PaymentId = resource.PaymentAccountId,
                Month = DateTime.Now.Month,
                Year = DateTime.UtcNow.Year
            };

            quotation.SubmitStatus(resource.Status);
            quotation.Products = MutateResourceProduct(resource.Products);
            quotation.Projects = MutateResourceProject(resource.Projects);

            var result = await this.Calculate(resource.Products);

            quotation.Products = result.QuotationProductEntities;

            quotation.Profit = result.Profit;
            quotation.Price = result.Price;
            quotation.Vat = result.Vat;
            quotation.Amount = result.Amount;
            quotation.AmountBeforeVat = result.AmountBeforeVat;
            quotation.SumOfDiscount = result.SumOfDiscount;
            quotation.RealPriceMsrp = result.RealPriceMsrp;

            _quotationRepository.Add(quotation);

            try
            {
                await _quotationRepository.Context().SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var entity = await _quotationRepository.GetQuotationQuery()
                .FirstOrDefaultAsync(x => x.QuotationId == quotation.QuotationId);

            if (entity == null)
            {
                throw new Exception("not quotation exist");
            }

            var response = await MapEntityToResponse(entity);

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<QuotationResource> Update(Guid id, QuotationResource resource)
    {
        if (resource is null)
            throw new ArgumentNullException(nameof(resource));

        // ป้องกัน null collection เข้ามา
        var incomingProducts = resource.Products ?? new List<QuotationProductResource>();
        var incomingProjects = resource.Projects ?? new List<QuotationProjectResource>();

        var quotation = await _quotationRepository
            .GetQuotationQuery()
            .FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
            throw new KeyNotFoundException("id not exists");

        // ปลอดภัยกับ string null (ให้เป็น ""), หรือจะคง null ก็ได้แล้วแต่สเกมา DB ของคุณ
        quotation.CustomerId = resource.CustomerId; // ถ้าเป็น Guid? ให้ตรวจเพิ่มถ้าจำเป็น
        quotation.CustomerContactId = resource.ContactPersonId;
        quotation.SalePersonId = resource.SalesPersonId;
        quotation.IssuedById = resource.IssuedById;
        quotation.Remark = resource.Remark ?? string.Empty;
        quotation.BusinessId = resource.BusinessId;
        quotation.Status = resource.Status ?? string.Empty;
        quotation.PaymentId = resource.PaymentAccountId;

        // ถ้า SubmitStatus ต้องการค่าไม่ว่าง ให้เช็คก่อน
        if (!string.IsNullOrWhiteSpace(resource.Status))
            quotation.SubmitStatus(resource.Status);

        // เก็บของเดิมไว้แบบปลอดภัย (ถ้า null ให้เป็นลิสต์ว่าง)
        var temp = quotation.Products ?? new List<QuotationProductEntity>();

        // ลบของเดิมแบบ null-safe
        if (quotation.Products != null && quotation.Products.Count > 0)
            _quotationRepository.DeleteProduct(quotation.Products);

        if (quotation.Projects != null && quotation.Projects.Count > 0)
            _quotationRepository.DeleteProject(quotation.Projects);

        // map ของใหม่ (ถ้า mapper คืน null ให้เป็นลิสต์ว่าง)
        quotation.Products = MutateResourceProduct(incomingProducts) ?? new List<QuotationProductEntity>();
        quotation.Projects = MutateResourceProject(incomingProjects) ?? new List<QuotationProjectEntity>();

        // ถ้าไม่มีสินค้า ก็ไม่ต้องคำนวณ (รีเซ็ตยอดเป็น 0)
        if (incomingProducts.Count == 0)
        {
            quotation.Price = 0m;
            quotation.Vat = 0m;
            quotation.Amount = 0m;
            quotation.AmountBeforeVat = 0m;
            quotation.SumOfDiscount = 0m;
            quotation.RealPriceMsrp = 0m;
            quotation.Update();

            var ctx0 = _quotationRepository.Context();
            if (ctx0 == null) throw new InvalidOperationException("DbContext is not available.");
            await ctx0.SaveChangesAsync();

            return await MapEntityToResponse(quotation);
        }

        // คำนวณแบบ null-safe
        var result = await this.Calculate(incomingProducts);
        if (result == null)
            throw new InvalidOperationException("Calculate() returned null.");

        var computedItems = result.QuotationProductEntities ?? new List<QuotationProductEntity>();

        // เตรียม dictionary ไว้หา item เดิมเร็วขึ้น และกัน temp เป็น null
        var oldByProductId = (temp ?? new List<QuotationProductEntity>())
            .Where(x => x != null)
            .GroupBy(x => x.ProductId)
            .ToDictionary(g => g.Key, g => g.First());

        // อัดค่าบางช่องจากของเดิม ถ้ามี (กัน productItem == null)
        foreach (var item in computedItems)
        {
            if (item == null) continue;
            if (item.ProductId == Guid.Empty) continue;

            if (oldByProductId.TryGetValue(item.ProductId, out var productItem) && productItem != null)
            {
                item.Currency = productItem.Currency;
                item.PurchasingPrice = productItem.PurchasingPrice;
                item.WHT = productItem.WHT;
                item.Exchange = productItem.Exchange;
                item.Incoterm = productItem.Incoterm;
                item.ImportDuty = productItem.ImportDuty;
                item.AdministrativeCosts = productItem.AdministrativeCosts;
                item.CostEstimate = productItem.CostEstimate;
                item.Order = productItem.Order;
            }
            else
            {
                item.Order = 0;
            }
        }

        quotation.Products = computedItems;
        quotation.Price = result.Price;
        quotation.Vat = result.Vat;
        quotation.Amount = result.Amount;
        quotation.AmountBeforeVat = result.AmountBeforeVat;
        quotation.SumOfDiscount = result.SumOfDiscount;
        quotation.RealPriceMsrp = result.RealPriceMsrp;

        quotation.Update();

        var ctx = _quotationRepository.Context();
        if (ctx == null) throw new InvalidOperationException("DbContext is not available.");

        try
        {
            await ctx.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InvalidOperationException("Failed to save changes.", e);
        }

        return await MapEntityToResponse(quotation);
    }


    public Task<List<QuotationStatus>> QuotationStatus()
    {
        return Task.FromResult(new List<QuotationStatus>()
        {
            new() { Status = "เสนอราคา" },
            new() { Status = "ปิดการขาย" },
            new() { Status = "อนุมัติ" },
            new() { Status = "ยกเลิก" },
        });
    }

    public async Task<QuotationResource> UpdateStatus(Guid id, QuotationResource status)
    {
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        quotation.SubmitStatus(status.Status);

        await _quotationRepository.Context()!.SaveChangesAsync();

        return await MapEntityToResponse(quotation);
    }


    public async Task Process(Guid id)
    {
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        quotation.Process();

        await _quotationRepository.Context()!.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        quotation.Process();

        await _quotationRepository.Context()!.SaveChangesAsync();
    }

    public async Task<QuotationResponse> Calculate(List<QuotationProductResource> resource)
    {
        decimal amount;
        decimal vat;
        decimal price = 0;
        decimal realPriceMsrp = 0;
        decimal sumOfDiscount = 0;
        decimal amountBeforeVat = 0;
        decimal profit = 0;
        bool isSpecialPrice = false;

        var products = MutateResourceProduct(resource);

        var response = new QuotationResponse();
        response.QuotationProductEntities = new List<QuotationProductEntity>();

        foreach (var product in products)
        {
            var selected = await _productRepository.GetProductListQueryable()
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (selected == null)
                throw new KeyNotFoundException("product not exists");

            // ===== คำนวณ MSRP/ส่วนลด/ก่อน VAT =====
            var qty = (decimal)product.Quantity;
            var msrpUnit = (decimal)selected.MSRP;
            var amountPU = (decimal)product.Amount;

            var realPrice = msrpUnit * qty; // MSRP รวม
            realPriceMsrp += realPrice;

            var dis = (msrpUnit - amountPU) * qty; // ส่วนลดรวมเทียบกับ MSRP
            sumOfDiscount += dis;

            product.SumOfDiscount = dis;
            product.AmountBeforeVat = realPrice - dis;
            product.RealPriceMsrp = realPrice;

            // ถ้าต้องสะสมรายการ
            response.QuotationProductEntities.Add(product);

            // เช็ค special price ต่อหน่วยจาก LwPrice ของ selected
            if (!isSpecialPrice)
                isSpecialPrice = amountPU < (decimal)selected.LwPrice;
        }

        amountBeforeVat = realPriceMsrp - sumOfDiscount;

        price = (decimal)products.Sum(x => x.Amount * x.Quantity);

        amount = price * (decimal)1.07;
        vat = amount - price;

        profit = (100 * amountBeforeVat) / realPriceMsrp;

        response.IsSpecialPrice = isSpecialPrice;
        response.Profit = profit;
        response.Amount = amount;
        response.Vat = vat;
        response.Price = price;
        response.AmountBeforeVat = amountBeforeVat;
        response.SumOfDiscount = sumOfDiscount;
        response.RealPriceMsrp = realPriceMsrp;

        return response;
    }

    public async Task<List<PaymentAccountResponse>> GetPaymentAccountListByBusiness(string orgId, Guid businessId)
    {
        _organizationRepository.SetCustomOrgId(orgId);
        var organization = await _organizationRepository.GetOrganization();
        var result = await _paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId)
            .Where(x => x.AccountStatus == RecordStatus.Active.ToString())
            .OrderBy(x => x.BankId).ToListAsync();
        return result.Select(x => new PaymentAccountResponse
        {
            PaymentAccountId = x.PaymentAccountId,
            OrgId = x.OrgId,
            PaymentAccountName = x.PaymentAccountName,
            AccountType = x.AccountType,
            AccountBank = x.BankId,
            // AccountBankName = x.AccountBank,
            AccountBrn = x.BankBranchId,
            // AccountBankBrn = x,AccountBankBrn,
            PaymentAccountNo = x.PaymentAccountNo,
            AccountStatus = "Active"
        }).ToList();
    }

    public async Task<PaymentAccountResponse> GetPaymentAccountInformationById(string orgId, Guid businessId,
        Guid paymentAccountId)
    {
        _organizationRepository.SetCustomOrgId(orgId);
        var organization = await _organizationRepository.GetOrganization();
        var result = await _paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId)
            .Where(x => x.PaymentAccountId == paymentAccountId).FirstOrDefaultAsync();
        return _mapper.Map<PaymentAccountEntity, PaymentAccountResponse>(result);
    }

    public async Task<PurchaseDetail> GetProductPurchaseDetail(Guid quotationId, Guid productId)
    {
        var product = await _quotationRepository.GetQuotationProduct(quotationId, productId)
            .FirstOrDefaultAsync(x => x.QuotationId == quotationId && x.ProductId == productId);
        var productItem = await _productRepository.GetProductById(productId).FirstOrDefaultAsync();
        var res = new PurchaseDetail();
        res.Amount = product.Amount;
        res.Currency = product.Currency;
        res.BuyUnitEstimate = product.PurchasingPrice.ToString("0.00", CultureInfo.InvariantCulture);
        res.ExchangeRate = product.Exchange.ToString("0.00", CultureInfo.InvariantCulture);
        res.Incoterm = product.Incoterm;
        res.CostsEstimate = product.CostEstimate.ToString("0.00", CultureInfo.InvariantCulture);
        res.OfferPriceLatest = product.LatestCost.ToString();
        res.ProductId = product.ProductId;
        res.AdministrativeCosts = product.AdministrativeCosts.ToString("0.00", CultureInfo.InvariantCulture);
        res.ImportDuty = product.ImportDuty.ToString("0.00", CultureInfo.InvariantCulture);
        res.WHT = product.WHT.ToString("0.00", CultureInfo.InvariantCulture);
        res.CurrencyInHand = productItem.CurrencyInhand;
        res.BuyUnitInHand = productItem.CostInhand.GetValueOrDefault().ToString("0.00", CultureInfo.InvariantCulture);
        res.CurrencyLatest = productItem.CurrencyLastPO;
        res.BuyUnitLatest = productItem.CostLastPO.GetValueOrDefault().ToString("0.00", CultureInfo.InvariantCulture);
        return res;
    }

    public async Task<PagedList<QuotationResponse>> GetByList(
        string keyword, Guid businessId, string? startDate, string? endDate,
        Guid? customerId, Guid? projectId, int? profit, bool? isSpecialPrice, Guid? salePersonId,
        string? status, int page, int pageSize, bool? isGreaterThan)
    {
        // --- helpers (throw only) ---
        static string Dump(object o) => JsonSerializer.Serialize(
            o,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

        Exception Boom(string stage, object details, Exception? ex = null)
        {
            var payload = new
            {
                stage,
                details,
                ex = ex == null
                    ? null
                    : new
                    {
                        type = ex.GetType().FullName,
                        msg = ex.Message,
                        stack = ex.StackTrace,
                        inner = ex.InnerException == null
                            ? null
                            : new
                            {
                                type = ex.InnerException.GetType().FullName,
                                msg = ex.InnerException.Message,
                                stack = ex.InnerException.StackTrace
                            }
                    }
            };
            return new InvalidOperationException(Dump(payload));
        }

        var inputSnapshot = new
        {
            keyword,
            businessId,
            startDate,
            endDate,
            customerId,
            projectId,
            profit,
            isSpecialPrice,
            salePersonId,
            status,
            page,
            pageSize,
            isGreaterThan
        };

        try
        {
            // ---------- normalize ----------
            var kw = string.IsNullOrWhiteSpace(keyword) ? null : keyword.Trim().ToLowerInvariant();

            DateTime? start = !string.IsNullOrEmpty(startDate)
                ? DateTime.SpecifyKind(DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    DateTimeKind.Utc)
                : null;

            DateTime? end = !string.IsNullOrEmpty(endDate)
                ? DateTime.SpecifyKind(DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    DateTimeKind.Utc)
                : null;

            var userId = _userPrincipalHandler.Id;

            var user = _businessRepository.GetUserBusinessQuery()
                .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                throw Boom("resolve-user", new { inputSnapshot, userId, reason = "user-business mapping not found" });

            // ---------- build query ----------
            // จุดสำคัญ: IgnoreAutoIncludes() กัน EF ไป INNER JOIN Customer/CustomerContact ให้เอง
            IQueryable<Entities.QuotationEntity> query = _quotationRepository
                .GetQuotationQuery()
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .Include(x => x.Projects) // ยัง LEFT JOIN Projects เหมือนเดิม
                .Where(x => x.BusinessId == businessId);

            var isPrivileged = !string.IsNullOrWhiteSpace(user.Role) &&
                               (user.Role.Contains("SaleManager") ||
                                user.Role.Contains("Director") ||
                                user.Role.Contains("Admin"));

            if (isPrivileged)
            {
                if (salePersonId.HasValue)
                    query = query.Where(x => x.SalePersonId == salePersonId.Value);
            }
            else
            {
                query = query.Where(x => x.SalePersonId == user.UserId);
            }

            if (kw != null)
                query = query.Where(x => x.QuotationNo != null &&
                                         (x.QuotationNo.ToLower().Contains(kw) || x.QuotationNo.ToLower() == kw));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(x => x.Status == status);

            if (start.HasValue)
                query = query.Where(x => x.QuotationDateTime.Date >= start.Value.Date);
            if (end.HasValue)
                query = query.Where(x => x.QuotationDateTime.Date <= end.Value.Date);

            if (customerId.HasValue)
                query = query.Where(x => x.CustomerId == customerId.Value);

            if (projectId.HasValue)
                query = query.Where(x => x.Projects.Any(p => p.ProjectId == projectId.Value));

            if (isSpecialPrice.HasValue)
                query = query.Where(x => x.IsSpecialPrice == isSpecialPrice.Value);

            if (profit.HasValue && isGreaterThan.HasValue)
                query = isGreaterThan.Value
                    ? query.Where(x => x.Profit >= profit.Value)
                    : query.Where(x => x.Profit < profit.Value);

            query = query.OrderByDescending(x => x.QuotationNo);

            // ---------- capture SQL ----------
            string? sql = null;
            try
            {
                sql = query.ToQueryString();
            }
            catch
            {
            }

            // ---------- page ----------
            PagedList<Entities.QuotationEntity> beforeMutate;
            try
            {
                beforeMutate = await PagedList<Entities.QuotationEntity>.Create(query, page, pageSize);
            }
            catch (Exception exPage)
            {
                throw Boom("page-db", new { inputSnapshot, isPrivileged, sql }, exPage);
            }

            if (beforeMutate.Items == null)
                throw Boom("page-null-items", new { inputSnapshot, isPrivileged, sql });

            if (beforeMutate.Items.Count == 0)
                throw Boom("no-results", new
                {
                    inputSnapshot,
                    isPrivileged,
                    userRole = user.Role,
                    appliedFilters = new
                    {
                        kw,
                        status,
                        dateFilter = new { start = start?.ToString("yyyy-MM-dd"), end = end?.ToString("yyyy-MM-dd") },
                        customerId,
                        projectId,
                        isSpecialPrice,
                        profit,
                        isGreaterThan,
                        salePersonIdWhenPrivileged = isPrivileged ? salePersonId : (Guid?)null,
                        salePersonEnforcedWhenNonPrivileged = !isPrivileged ? user.UserId : (Guid?)null
                    },
                    sql
                });

            // ---------- map with per-row guard ----------
            var list = new List<QuotationResponse>(beforeMutate.Items.Count);
            var mapErrors = new List<object>();

            decimal SumAmtQty(IEnumerable<dynamic>? items)
            {
                decimal acc = 0m;
                foreach (var it in items ?? Enumerable.Empty<dynamic>())
                {
                    decimal amt = it?.Amount == null ? 0m : Convert.ToDecimal(it.Amount);
                    decimal qty = it?.Quantity == null ? 0m : Convert.ToDecimal(it.Quantity);
                    acc += amt * qty;
                }

                return acc;
            }

            decimal SumNum(IEnumerable<dynamic>? items)
            {
                decimal acc = 0m;
                foreach (var it in items ?? Enumerable.Empty<dynamic>())
                {
                    decimal amt = it?.Amount == null ? 0m : Convert.ToDecimal(it.Amount);
                    decimal cost = it?.CostEstimate == null ? 0m : Convert.ToDecimal(it.CostEstimate);
                    decimal qty = it?.Quantity == null ? 0m : Convert.ToDecimal(it.Quantity);
                    acc += (amt - cost) * qty;
                }

                return acc;
            }

            for (int i = 0; i < beforeMutate.Items.Count; i++)
            {
                var x = beforeMutate.Items[i];
                try
                {
                    var products = x.Products as IEnumerable<dynamic>;
                    var den = SumAmtQty(products);
                    var num = SumNum(products);
                    var profitPct = den == 0m ? 0m : (num * 100m) / den;

                    var saleName = string.Join(" ", new[]
                    {
                        x.SalePerson?.FirstNameTh ?? "",
                        x.SalePerson?.LastnameTh ?? ""
                    }.Where(s => !string.IsNullOrWhiteSpace(s))).Trim();
                    if (string.IsNullOrWhiteSpace(saleName)) saleName = "-";

                    list.Add(new QuotationResponse
                    {
                        QuotationId = x.QuotationId ?? Guid.Empty,
                        CustomerId = x.CustomerId,
                        CustomerNo = x.Customer?.No, // จะเป็น null ได้ถ้าไม่ได้โหลด ก็โอเค
                        CustomerName = x.Customer?.DisplayName, // ป้องกัน null แล้ว fallback ฝั่ง UI
                        Address = x.Customer?.Address(),
                        ContactPerson = x.CustomerContact?.DisplayName(),
                        ContactPersonId = x.CustomerContactId,
                        QuotationNo = x.QuotationNo ?? "-",
                        QuotationDateTime = x.QuotationDateTime.ToString("dd/MM/yyyy"),
                        EditTime = x.EditTime,
                        IssuedByUser = null,
                        IssuedByUserId = x.IssuedById,
                        IssuedByUserName = x.IssuedByUser?.Username ?? "-",
                        SalePersonId = x.SalePersonId,
                        SalePersonName = saleName,
                        Status = x.Status,
                        Products = null,
                        ProjectName = x.Projects?.FirstOrDefault()?.Project?.ProjectName,
                        EthSaleMonth = x.Projects?.FirstOrDefault()?.EthSaleMonth?.ToString("MM/yyyy"),
                        TotalOffering = den,
                        Projects = null,
                        Price = x.RealPriceMsrp,
                        Vat = x.SumOfDiscount,
                        // แก้เรื่อง ?? กับ decimal non-nullable
                        Amount = (((decimal?)x.RealPriceMsrp) ?? 0m) - (((decimal?)x.SumOfDiscount) ?? 0m),
                        Remark = x.Remark,
                        Profit = profitPct,
                        IsSpecialPrice = x.IsSpecialPrice
                    });
                }
                catch (Exception exRow)
                {
                    mapErrors.Add(new
                    {
                        index = i,
                        quotationId = x?.QuotationId,
                        ex = new { type = exRow.GetType().FullName, msg = exRow.Message, stack = exRow.StackTrace }
                    });
                }
            }

            if (mapErrors.Count > 0)
            {
                throw Boom("map-errors", new
                {
                    succeeded = list.Count,
                    failed = mapErrors.Count,
                    mapErrors,
                    sql
                });
            }

            return new PagedList<QuotationResponse>(list, beforeMutate.TotalCount, page, pageSize);
        }
        catch (Exception ex)
        {
            throw Boom("fatal", new { inputSnapshot }, ex);
        }
    }


    public async Task<TotalProductQuotation> GetTotalProductQuotation(Guid id)
    {
        var query = await _quotationRepository.GetQuotationQuery()
            .Where(x => x.QuotationId == id)
            .Include(x => x.Products)
            .SingleOrDefaultAsync();

        var products = query?.Products;


        return new TotalProductQuotation
        {
            QuotationId = id,
            TotalAmount = products.Sum(x => x.Amount * x.Quantity),
            TotalCost = 0,
            Profit = 0,
            TotalProfitPercent = 0,
            TotalEstimate = products.Sum(x => x.CostEstimate),
            TotalEstimatePercent = 0,
            TotalEstimateProfit = products.Sum(x => x.CostEstimateProfit),
            TotalEstimateProfitPercent = 0
        };
    }

    public async Task ImportUpdate(Guid productId)
    {
        // Load
        var product = await _productRepository.GetProductById(productId).FirstOrDefaultAsync();
        if (product is null) throw new InvalidOperationException($"Product {productId} not found.");

        var quotations = await _quotationRepository.GetQuotationProducts(productId).ToListAsync();
        if (quotations.Count == 0) return;

        var errors = new List<string>();
        decimal purchasingPrice = RequireNonNegative(product.BuyUnitEst, nameof(product.BuyUnitEst), errors);
        decimal exchange = RequireGreaterThan(product.ExchangeRateEst, 0m, nameof(product.ExchangeRateEst), errors);
        string incoterm = RequireIncoterm(product.IncortermEst, nameof(product.IncortermEst), errors);
        decimal adminCostsPct =
            RequirePercent(product.AdministrativeCostEst, nameof(product.AdministrativeCostEst), errors);
        decimal importDutyPct = RequirePercent(product.ImportDutyEst, nameof(product.ImportDutyEst), errors);
        decimal whtPct = RequirePercent(product.WHTEst, nameof(product.WHTEst), errors);

        if (whtPct >= 100m) errors.Add("WHTEst must be < 100.");
        if (errors.Count > 0) throw new ArgumentException(string.Join("; ", errors));

        static decimal R(decimal v, int s) => Math.Round(v, s, MidpointRounding.AwayFromZero);

        // ราคาซื้อ/หน่วย (หลังหัก WHT ที่กรอก)
        var value = purchasingPrice;

        // gross-up กลับไปก่อนหัก WHT
        var amountEstimate = value * 100m / (100m - whtPct);
        var whtEstimate = amountEstimate - value;

        // แปลงเป็น THB
        var amountTHB = amountEstimate * exchange;

        // ภาษี/แอดมิน
        var importDutyAmt = amountTHB * (importDutyPct / 100m);
        var adminCostsAmt = amountTHB * (adminCostsPct / 100m);

        var costsEstimate = amountTHB + importDutyAmt + adminCostsAmt;

        // กำไรขั้นต่ำ 25%
        const decimal minMargin = 25m;
        var lowerPriceEstimate = costsEstimate * 100m / (100m - minMargin);
        var offerPriceEstimate = lowerPriceEstimate;

        Console.WriteLine($"Offerning Price : {offerPriceEstimate}");
        foreach (var quotation in quotations)
        {
            Console.WriteLine(
                $"Quotation Estimate (Decimal) : {quotation.CostEstimate}"); // :R จะโชว์ค่าจริงแบบ round-trip

            if (quotation.CostEstimate > 0)
                continue;

            Console.WriteLine($"Quotation Id : {quotation.QuotationId}");

            // ส่วนลดจาก MSRP เทียบกับราคาเสนอ (บาท)
            if (product.MSRP is decimal msrp && msrp > 0m)
                quotation.SumOfDiscount = msrp - offerPriceEstimate;
            else
                quotation.SumOfDiscount = 0m;

            quotation.Currency = product.CurrencyEst;
            quotation.Profit = 0m;
            quotation.ProfitPercent = 0m;

            quotation.PurchasingPrice = purchasingPrice;
            quotation.Exchange = exchange;
            quotation.Incoterm = incoterm;
            quotation.CostEstimate = R(costsEstimate, 2);
            quotation.AdministrativeCosts = adminCostsPct;
            quotation.ImportDuty = importDutyPct;
            quotation.WHT = whtPct;

            _quotationRepository.UpdateProduct(quotation);
        }

        await _quotationRepository.Context().SaveChangesAsync();
    }

    public async Task<QuotationResource> UpdateCostEstimateQuotation(Guid id, Guid productId,
        UpdateProductQuotationParameter request)
    {
        var product = await _quotationRepository.GetQuotationProduct(id, productId).FirstOrDefaultAsync();
        var quotaion = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);
        var productItem = await _productRepository.GetProductByBusiness(quotaion!.BusinessId)
            .FirstOrDefaultAsync(x => x.ProductId == productId);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        product.SumOfDiscount = (decimal)productItem.MSRP - decimal.Parse(request.Data.OfferPriceEstimate,
            NumberStyles.Any, CultureInfo.InvariantCulture);
        product.Currency = request.Data.Currency;
        if (float.TryParse(request.Data.OfferPriceEstimate, NumberStyles.Any, CultureInfo.InvariantCulture, out var offeringPrice))
        {
            product.Amount = offeringPrice;               
            _quotationRepository.Context().Entry(product).Property(p => p.Amount).IsModified = true;  // บังคับ mark modified
        }
        product.LatestCost = decimal.TryParse(request.Data.OfferPriceEstimate, NumberStyles.Any,
            CultureInfo.InvariantCulture, out var latestCost)
            ? latestCost
            : 0m;
        product.Profit = 0;
        product.ProfitPercent = 0;
        product.PurchasingPrice = decimal.TryParse(request.Data.BuyUnitEstimate, NumberStyles.Any,
            CultureInfo.InvariantCulture, out var purchasingPrice)
            ? purchasingPrice
            : 0m;
        product.Exchange = decimal.TryParse(request.Data.ExchangeRate, NumberStyles.Any, CultureInfo.InvariantCulture,
            out var exchange)
            ? exchange
            : 0m;
        product.Incoterm = request.Data.Incoterm;
        product.CostEstimate = decimal.TryParse(request.Data.CostEstimate, NumberStyles.Any,
            CultureInfo.InvariantCulture, out var costEstimate)
            ? costEstimate
            : 0m;
        product.AdministrativeCosts = decimal.TryParse(request.Data.AdministrativeCosts, NumberStyles.Any,
            CultureInfo.InvariantCulture, out var administrativeCosts)
            ? administrativeCosts
            : 0m;
        product.ImportDuty =
            decimal.TryParse(request.Data.ImportDuty, NumberStyles.Any, CultureInfo.InvariantCulture,
                out var importDuty)
                ? importDuty
                : 0m;
        product.WHT = decimal.TryParse(request.Data.Wht, NumberStyles.Any, CultureInfo.InvariantCulture, out var wht)
            ? wht
            : 0m;

        _quotationRepository.UpdateProduct(product);

        await _quotationRepository.Context().SaveChangesAsync();

        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        return await MapEntityToResponse(quotation);
    }

    public async Task<QuotationResource> GetById(Guid id)
    {
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        return await MapEntityToResponse(quotation);
    }

    public async Task<QuotationResource> ApproveSalePrice(Guid id)
    {
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        await SendEmail(quotation, Emails);


        return null;
    }

    public async Task<QuotationResource> ApproveQuotation(Guid id)
    {
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        quotation.SetApprove();

        _quotationRepository.Context().Update(quotation);

        await _quotationRepository.Context().SaveChangesAsync();


        try
        {
            await SendApproveQuotation(quotation, Emails);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw e;
        }


        return null;
    }

    public async Task<QuotationDocument> GeneratePDF(Guid id)
    {
        var quotation = await _quotationRepository.GetQuotationQuery()
            .Include(x => x.Business)
            .Include(x => x.PaymentAccountEntity)
            .ThenInclude(x => x.BankEntity)
            .Include(x => x.PaymentAccountEntity)
            .ThenInclude(x => x.BankBranchEntity)
            .FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        var business = _businessRepository.GetBusinessesQuery()
            .FirstOrDefault(x => x.BusinessId == quotation.BusinessId);

        if (business == null)
        {
            throw new KeyNotFoundException("business id not exists");
        }

        var query = business;

        var orgAddress = (string.IsNullOrEmpty(query.Building) ? "" : "อาคาร " + (query.Building + " ")) +
                         (string.IsNullOrEmpty(query.RoomNo) ? "" : "ห้อง " + (query.RoomNo + " ")) +
                         (string.IsNullOrEmpty(query.Floor) ? "" : "ชั้น " + (query.Floor + " ")) +
                         (string.IsNullOrEmpty(query.Village) ? "" : "หมู่บ้่าน " + (query.Village + " ")) +
                         (string.IsNullOrEmpty(query.No) ? "" : "เลขที่ " + (query.No + " ")) +
                         (string.IsNullOrEmpty(query.Moo) ? "" : "หมู่ " + (query.Moo + " ")) +
                         (string.IsNullOrEmpty(query.Alley) ? "" : "ซอย " + (query.Alley + " ")) +
                         (string.IsNullOrEmpty(query.Road) ? "" : "ถนน " + (query.Road + " ")) +
                         (string.IsNullOrEmpty(query.SubDistrict)
                             ? ""
                             : "แขวง " + (_systemRepository.GetAll<SubDistrictEntity>()
                                 .Where(x => x.SubDistrictCode.ToString().Equals(query.SubDistrict)).FirstOrDefault()
                                 .SubDistrictNameTh + " ")) +
                         (string.IsNullOrEmpty(query.District)
                             ? ""
                             : "เขต " + (_systemRepository.GetAll<DistrictEntity>()
                                 .Where(x => x.DistrictCode.ToString().Equals(query.District)).FirstOrDefault()
                                 .DistrictNameTh + " ")) +
                         (string.IsNullOrEmpty(query.Province)
                             ? ""
                             : "จังหวัด " + (_systemRepository.GetAll<ProvinceEntity>()
                                 .Where(x => x.ProvinceCode.ToString().Equals(query.Province)).FirstOrDefault()
                                 .ProvinceNameTh + " ")) +
                         (string.IsNullOrEmpty(query.PostCode) ? "" : "รหัสไปรษณีย์ " + query.PostCode);

        var queryCustomer = quotation.Customer;
        var cusAddress = queryCustomer.CusFullAddress;

        return new QuotationDocument(quotation, business, orgAddress, cusAddress);
    }

    public async Task DeleteAll()
    {
        var query = await _quotationRepository.GetQuotationQuery().ToListAsync();

        _quotationRepository.DeleteAll(query);

        await _quotationRepository.Context().SaveChangesAsync();
    }

    private async Task SendApproveQuotation(QuotationEntity quotation, List<EmailInformation> list)
    {
        var apiInstance = new TransactionalEmailsApi();
        string SenderName = "PROM ERP";
        string SenderEmail = "e-service@prom.co.th";
        SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
        List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();

        foreach (var email in list)
        {
            string ToEmail = email.Email;
            string ToName = email.Name;
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
            To.Add(smtpEmailTo);
        }

        var link =
            $"<a href = 'https://sales.prom.co.th/erp/quotation/form/{quotation.QuotationId}'>{quotation.QuotationNo}</a>";

        string HtmlContent =
            $"เร\u0e37\u0e48อง ขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49ใบเสนอราคา<br/>" +
            $"เร\u0e35ยน " +
            // $"{managerName}</br>" +
            $"<dd>เน\u0e37\u0e48องจากในขณะน\u0e35\u0e49เอกสารใบเสนอราคาเลขท\u0e35\u0e48: {link ?? ""} ได\u0e49ถ\u0e39กจ\u0e31ดทำเสร\u0e47จเร\u0e35ยบร\u0e49อยแล\u0e49ว จ\u0e36งนำเสนอมาเพ\u0e37\u0e48อขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49รายละเอ\u0e35ยดท\u0e31\u0e49งหมดตามในเอกสารด\u0e31งกล\u0e48าวและจะได\u0e49" +
            $"ดำเน\u0e34นการเสนอราคาแก\u0e48ล\u0e39กค\u0e49าต\u0e48อไป\n<br/><br/><br/>\n" +
            $"จ\u0e36งเร\u0e35ยนมาเพ\u0e37\u0e48อโปรดพ\u0e34จารณา<br/>\n" +
            $"{quotation.IssuedByUser.DisplayNameTH()}<br/>";
        string Subject = @$"ขออนุมัติราคา ใบเสนอราคาเลขที่ {quotation.QuotationNo ?? "-"}";

        try
        {
            var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, null, Subject);
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task SendGeneralMail(string to, string toName, string subject, string body)
    {
        var apiInstance = new TransactionalEmailsApi();
        string SenderName = "PROM ERP";
        string SenderEmail = "e-service@prom.co.th";
        SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
        List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();

        string ToEmail = to;
        string ToName = toName;
        SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
        To.Add(smtpEmailTo);


        string HtmlContent = body;
        string Subject = subject;

        try
        {
            var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, null, Subject);
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private async Task ManagerReplyApproveQuotation(QuotationEntity quotation, string saleName, string saleEmail)
    {
        var apiInstance = new TransactionalEmailsApi();
        string senderName = "PROM ERP";
        string senderEmail = "e-service@prom.co.th";

        SendSmtpEmailSender Email = new SendSmtpEmailSender(senderName, senderEmail);

        string toEmail = saleEmail;
        string toName = saleName;
        SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(toEmail, toName);

        List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
        To.Add(smtpEmailTo);

        string htmlContent =
            $"ตามท\u0e35\u0e48ได\u0e49ม\u0e35การขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49ใบเสนอราคาเลขท\u0e35\u0e48: {quotation.QuotationNo ?? ""} ขณะน\u0e35\u0e49ใบเสนอราคาด\u0e31งกล\u0e48าวได\u0e49ถ\u0e39กอน\u0e38ม\u0e31ต\u0e34เร\u0e35ยบร\u0e49อยแล\u0e49ว จ\u0e36งแจ\u0e49งกล\u0e31บมาเพ\u0e37\u0e48อให\u0e49ดำเน\u0e34นการตามข\u0e31\u0e49นตอนต\u0e48างๆต\u0e48อไป";
        string Subject = @$"อนุมัติใช้ใบเสนอราคา";

        try
        {
            var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, htmlContent, null, Subject);
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private async Task SendApproveSalePrice(QuotationEntity quotation, string managerName, string managerEmail)
    {
        var apiInstance = new TransactionalEmailsApi();
        string senderName = "PROM ERP";
        string senderEmail = "e-service@prom.co.th";

        SendSmtpEmailSender Email = new SendSmtpEmailSender(senderName, senderEmail);

        string toEmail = quotation.CustomerContact!.Email!;
        string toName = quotation.CustomerContact!.DisplayName()!;
        SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(managerName, managerEmail);

        List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
        To.Add(smtpEmailTo);

        string htmlContent = "Text";
        string Subject = "My ERP";

        try
        {
            var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, htmlContent, null, Subject);
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }


    public static async Task SendEmail(QuotationEntity entity, List<EmailInformation> emails)
    {
        var apiInstance = new TransactionalEmailsApi();
        string SenderName = "PROM ERP";
        string SenderEmail = "e-service@prom.co.th";
        SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);

        List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();

        foreach (var email in emails)
        {
            string ToEmail = email.Email;
            string ToName = email.Name;
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
            To.Add(smtpEmailTo);
        }

        var link =
            $"<a href = 'https://sales.prom.co.th/erp/quotation/form/{entity.QuotationId}'>{entity.QuotationNo}</a>";


        string HtmlContent =
            $"เร\u0e37\u0e48อง ขออน\u0e38ม\u0e31ต\u0e34เสนอ ราคาส\u0e38ทธ\u0e34/หน\u0e48วย ท\u0e35\u0e48ต\u0e48ำกว\u0e48าท\u0e35\u0e48ถ\u0e39กกำหนด<br/>" +
            $"เร\u0e35ยน ผ\u0e39\u0e49ท\u0e35\u0e48เก\u0e35\u0e48ยวข\u0e49องท\u0e38กท\u0e48าน</br>" +
            $"<dd>เน\u0e37\u0e48องจากในขณะน\u0e35\u0e49ม\u0e35ความจำเป\u0e47นบางประการท\u0e35\u0e48จะต\u0e49องเสนอ ราคาส\u0e38ทธ\u0e34/หน\u0e48วย ท\u0e35\u0e48ต\u0e48ำกว\u0e48า" +
            $"ราคาต\u0e48ำส\u0e38ดซ\u0e36\u0e48งได\u0e49ถ\u0e39กกำหนดไว\u0e49ในระบบเพ\u0e37\u0e48อใช\u0e49เฉพาะก\u0e31บเอกสารใบเสนอราคาเลขท\u0e35\u0e48: " +
            $"{link}<br/><br/><br/>" +
            $"<dt>จ\u0e36งเร\u0e35ยนมาเพ\u0e37\u0e48อโปรดพ\u0e34จารณา<br/>\n" +
            $"{entity.SalePerson.DisplayNameTH()}<br/>";
        string Subject = @$"ขออนุมัติราคา ใบเสนอราคาเลขที่ {entity.QuotationNo ?? "-"}";

        try
        {
            var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, null, Subject);
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private static decimal RequireNonNegative(decimal? value, string field, List<string> errors)
    {
        if (value is null)
        {
            errors.Add($"{field} is required.");
            return 0m;
        }

        if (value < 0m)
            errors.Add($"{field} must be >= 0.");
        return value.Value;
    }

    private static decimal RequireGreaterThan(decimal? value, decimal minExclusive, string field, List<string> errors)
    {
        if (value is null)
        {
            errors.Add($"{field} is required.");
            return 0m;
        }

        if (value <= minExclusive)
            errors.Add($"{field} must be > {minExclusive.ToString(CultureInfo.InvariantCulture)}.");
        return value.Value;
    }

    private static decimal RequirePercent(decimal? value, string field, List<string> errors)
    {
        if (value is null)
        {
            errors.Add($"{field} (percent) is required.");
            return 0m;
        }

        if (value < 0m || value > 100m)
            errors.Add($"{field} must be between 0 and 100 (percent).");
        return value.Value;
    }

    private static string RequireIncoterm(string? value, string field, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add($"{field} is required (EXW/FOB/FCA/CFR/CIF/CPT/CIP/DAP/DPU/DDP).");
            return string.Empty;
        }

        var normalized = value.Trim().ToUpperInvariant();
        if (!ValidIncoterms.Contains(normalized))
            errors.Add($"{field} '{value}' is not a valid Incoterm 2020.");
        return normalized;
    }


    private static DateTime? ParseDate(string? ddMMyyyy)
    {
        if (string.IsNullOrWhiteSpace(ddMMyyyy)) return null;
        var dt = DateTime.ParseExact(ddMMyyyy, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
    }

    public async Task<QuotationDashboardResponse> GetDashboardAsync(Guid businessId, string? startDate, string? endDate)
    {
        var start = ParseDate(startDate);
        var end = ParseDate(endDate);

        // ===== base quotation (ผ่าน repo) =====
        var qset = _quotationRepository.Context().Set<QuotationEntity>().AsNoTracking();

        IQueryable<QuotationEntity> baseQuotation = qset
            .Where(q => q.BusinessId == businessId && q.Status != "Delete");

        if (start.HasValue) baseQuotation = baseQuotation.Where(q => q.QuotationDateTime.Date >= start.Value.Date);
        if (end.HasValue) baseQuotation = baseQuotation.Where(q => q.QuotationDateTime.Date <= end.Value.Date);

        var totalCount = await baseQuotation.CountAsync();
        var totalInclVat = await baseQuotation.SumAsync(q => (decimal?)q.Amount) ?? 0m; // รวม VAT
        var totalExVat = await baseQuotation.SumAsync(q => (decimal?)q.Price) ?? 0m; // ไม่รวม VAT

        // ===== Top Customers (ตาม Amount รวม VAT) =====
        var cusAgg = await baseQuotation
            .GroupBy(q => q.CustomerId)
            .Select(g => new { CustomerId = g.Key, Total = g.Sum(x => x.Amount) })
            .OrderByDescending(x => x.Total)
            .Take(5)
            .ToListAsync();

        var topCusIds = cusAgg.Select(x => x.CustomerId).ToList();

        // ดึงชื่อ customer ผ่าน repo ที่ include customer อยู่แล้ว
        var customerNames = await _quotationRepository.GetQuotationQuery()
            .AsNoTracking()
            .Where(q => topCusIds.Contains(q.CustomerId))
            .Select(q => new { q.CustomerId, Name = q.Customer != null ? q.Customer.DisplayName : null })
            .Distinct()
            .ToListAsync();

        var customerNameDict = customerNames
            .GroupBy(x => x.CustomerId)
            .ToDictionary(g => g.Key, g => g.Select(v => v.Name).FirstOrDefault());

        var topCustomers = cusAgg.Select(x => new TopCustomerItem
        {
            CustomerId = x.CustomerId,
            CustomerName = customerNameDict.TryGetValue(x.CustomerId, out var n) ? n : null,
            TotalAmount = x.Total
        }).ToList();

        // ===== Top Products (Amount * Quantity) =====
        var qpBase = _quotationRepository.Context().Set<QuotationProductEntity>().AsNoTracking()
            .Where(qp => qp.Quotation.BusinessId == businessId && qp.Quotation.Status != "Delete");

        if (start.HasValue) qpBase = qpBase.Where(qp => qp.Quotation.QuotationDateTime.Date >= start.Value.Date);
        if (end.HasValue) qpBase = qpBase.Where(qp => qp.Quotation.QuotationDateTime.Date <= end.Value.Date);

        var prodAgg = await qpBase
            .GroupBy(qp => qp.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Quantity = g.Sum(x => x.Quantity),
                Total = g.Sum(x => (decimal)x.Amount * (decimal)x.Quantity)
            })
            .OrderByDescending(x => x.Total)
            .Take(5)
            .ToListAsync();

        var prodIds = prodAgg.Select(x => x.ProductId).ToList();

        var productNames = await _quotationRepository.GetQuotationQuery()
            .AsNoTracking()
            .SelectMany(q => q.Products)
            .Where(p => prodIds.Contains(p.ProductId))
            .Select(p => new
            {
                p.ProductId,
                Name = p.Product != null ? p.Product.ProductName : null
            })
            .Distinct()
            .ToListAsync();

        var productNameDict = productNames
            .GroupBy(x => x.ProductId)
            .ToDictionary(g => g.Key, g => g.Select(v => v.Name).FirstOrDefault());

        var topProducts = prodAgg.Select(x => new TopProductItem
        {
            ProductId = x.ProductId,
            ProductName = productNameDict.TryGetValue(x.ProductId, out var n) ? n : null,
            Quantity = x.Quantity,
            TotalAmount = x.Total
        }).ToList();

        return new QuotationDashboardResponse
        {
            TotalQuotations = totalCount,
            TotalValueInclVat = totalInclVat,
            TotalValueExVat = totalExVat,
            TopProducts = topProducts,
            TopCustomers = topCustomers
        };
    }
}