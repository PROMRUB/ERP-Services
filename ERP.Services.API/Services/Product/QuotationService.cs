using System.Globalization;
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
        }
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

        return MapEntityToResponse(quotation);
    }

    public List<QuotationProductResource> MapProductEntityToResource(List<QuotationProductEntity> entities)
    {
        return entities.Select(x => new QuotationProductResource
        {
            Amount = x.Amount,
            ProductId = x.ProductId,
            Quantity = x.Quantity,
            Discount = Convert.ToInt32(x.Discount),
            Order = x.Order
        }).ToList();
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

    public QuotationResource MapEntityToResponse(QuotationEntity quotation)
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
                Products = MapProductEntityToResource(quotation.Products),
                Projects = MapProjectEntityToResource(quotation.Projects),
                Remark = quotation.Remark,
                PaymentAccountId = quotation.PaymentId
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

            var response = MapEntityToResponse(entity);

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
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        quotation.CustomerId = resource.CustomerId;
        quotation.CustomerContactId = resource.ContactPersonId;
        quotation.QuotationDateTime = DateTime.UtcNow;
        quotation.SalePersonId = resource.SalesPersonId;
        quotation.IssuedById = resource.IssuedById;
        quotation.Remark = resource.Remark;
        quotation.BusinessId = resource.BusinessId;
        quotation.Status = resource.Status;
        quotation.PaymentId = resource.PaymentAccountId;

        quotation.SubmitStatus(resource.Status);

        _quotationRepository.DeleteProduct(quotation.Products);
        _quotationRepository.DeleteProject(quotation.Projects);

        quotation.Products = MutateResourceProduct(resource.Products);
        quotation.Projects = MutateResourceProject(resource.Projects);


        var result = await this.Calculate(resource.Products);

        quotation.Products = result.QuotationProductEntities;

        quotation.Price = result.Price;
        quotation.Vat = result.Vat;
        quotation.Amount = result.Amount;
        quotation.AmountBeforeVat = result.AmountBeforeVat;
        quotation.SumOfDiscount = result.SumOfDiscount;
        quotation.RealPriceMsrp = result.RealPriceMsrp;
        quotation.Update();

        try
        {
            await _quotationRepository.Context()!.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        if (quotation.Status == "อนุมัติ")
        {
            try
            {
                await ManagerReplyApproveQuotation(quotation, quotation.SalePerson.DisplayNameTH(),
                    quotation.SalePerson.email);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        return MapEntityToResponse(quotation);
    }

    public Task<List<QuotationStatus>> QuotationStatus()
    {
        return Task.FromResult(new List<QuotationStatus>()
        {
            new() { Status = "เสนอราคา" },
            new() { Status = "อนุมัติ" },
            new() { Status = "ไม่อนุมัติ" },
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

        return MapEntityToResponse(quotation);
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

        var products = MutateResourceProduct(resource);

        var response = new QuotationResponse();
        response.QuotationProductEntities = new List<QuotationProductEntity>();

        foreach (var product in products)
        {
            var selected = await _productRepository.GetProductListQueryable()
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (selected == null)
            {
                throw new KeyNotFoundException("product not exists");
            }

            var realPrice = (decimal)selected.MSRP * product.Quantity;
            realPriceMsrp += realPrice;


            // if (product.Discount > 0)
            // {
            //     var dis = (decimal)(selected.MSRP * (decimal?)(product.Discount / 100))! * product.Quantity;
            //     sumOfDiscount += dis;
            //
            //     product.SumOfDiscount = dis;
            // }

            var dis = (selected.MSRP - (decimal?)product.Amount) * product.Quantity;
            sumOfDiscount += (decimal)dis;
            product.SumOfDiscount = (decimal)dis;

            product.AmountBeforeVat = realPrice - product.SumOfDiscount;
            product.RealPriceMsrp = realPrice;

            response.QuotationProductEntities.Add(product);
        }

        amountBeforeVat = realPriceMsrp - sumOfDiscount;

        price = (decimal)products.Sum(x => x.Amount * x.Quantity);

        amount = price * (decimal)1.07;
        vat = amount - price;


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

    public async Task<PagedList<QuotationResponse>> GetByList(string keyword, Guid businessId, int page, int pageSize)
    {
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.ToLower();
        }

        var userId = _userPrincipalHandler.Id;

        var user = _businessRepository.GetUserBusinessQuery()
            .FirstOrDefault(x => x.UserId == userId);

        var query = _quotationRepository.GetQuotationQuery()
                .Where(x => x.BusinessId == businessId)
                .Where(x => user.UserId == x.SalePersonId)
                .Where(x => (string.IsNullOrWhiteSpace(keyword) || x.Customer.CusName.Contains(keyword))
                            || (string.IsNullOrWhiteSpace(keyword) || x.QuotationNo.Contains(keyword))
                            || (string.IsNullOrWhiteSpace(keyword) ||
                                x.Products.Any(p => p.Product.ProductName.Contains(keyword))) &&
                            (string.IsNullOrWhiteSpace(keyword) ||
                             x.QuotationNo.Contains(keyword)))
                .OrderByDescending(x => x.QuotationNo)
            ;

        if (user != null && !string.IsNullOrWhiteSpace(user.Role) && user.Role.Contains("SaleManager") && user.Role.Contains("Director"))
        {
            query = _quotationRepository.GetQuotationQuery()
                    .Where(x => x.BusinessId == businessId)
                    .Where(x => (string.IsNullOrWhiteSpace(keyword) || x.Customer.CusName.Contains(keyword))
                                && (string.IsNullOrWhiteSpace(keyword) || x.QuotationNo.Contains(keyword))
                                && (string.IsNullOrWhiteSpace(keyword) ||
                                    x.Products.Any(p => p.Product.ProductName.Contains(keyword))) &&
                                (string.IsNullOrWhiteSpace(keyword) ||
                                 x.QuotationNo.Contains(keyword)))
                    .OrderByDescending(x => x.QuotationNo)
                ;
        }


        var beforeMutate = await PagedList<Entities.QuotationEntity>.Create(query, page, pageSize);


        var list = beforeMutate.Items.Select(x => new QuotationResponse
            {
                QuotationId = x.QuotationId.Value,
                CustomerId = x.CustomerId,
                CustomerNo = x.Customer.No,
                CustomerName = x.Customer.DisplayName,
                Address = x.Customer.Address(),
                ContactPerson = x.CustomerContact.DisplayName(),
                ContactPersonId = x.CustomerContactId,
                QuotationNo = x.QuotationNo,
                QuotationDateTime = x.QuotationDateTime.ToString("dd/MM/yyyy"),
                EditTime = x.EditTime,
                IssuedByUser = null,
                IssuedByUserId = x.IssuedById,
                IssuedByUserName = x.IssuedByUser.Username ?? "-",
                SalePersonId = x.SalePersonId,
                SalePersonIName = x.SalePerson.FirstNameTh ?? "-",
                Status = x.Status,
                Products = null,
                ProjectName = x.Projects.FirstOrDefault()?.Project.ProjectName,
                EthSaleMonth = x.Projects.FirstOrDefault()?.EthSaleMonth?.ToString("MM/yyyy"),
                Projects = null,
                Price = x.RealPriceMsrp,
                Vat = x.SumOfDiscount,
                Amount = x.RealPriceMsrp - x.SumOfDiscount,
                // AccountNo = x.PaymentId.Value,
                Remark = x.Remark,
            })
            .ToList();

        var afterMutate = new PagedList<QuotationResponse>(list, beforeMutate.TotalCount, page, pageSize);

        return afterMutate;
    }

    public async Task<QuotationResource> GetById(Guid id)
    {
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        return MapEntityToResponse(quotation);
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

        var orgAddress = (string.IsNullOrEmpty(query.Building) ? "" :"อาคาร " + (query.Building + " ")) +
                         (string.IsNullOrEmpty(query.RoomNo) ? "" :"ห้อง " + (query.RoomNo + " ")) +
                         (string.IsNullOrEmpty(query.Floor) ? "" :"ชั้น " + (query.Floor + " ")) +
                         (string.IsNullOrEmpty(query.Village) ? "" :"หมู่บ้่าน " + (query.Village + " ")) +
                         (string.IsNullOrEmpty(query.No) ? "" :"เลขที่ " + (query.No + " ")) +
                         (string.IsNullOrEmpty(query.Moo) ? "" :"หมู่ " + (query.Moo + " ")) +
                         (string.IsNullOrEmpty(query.Alley) ? "" :"ซอย " + (query.Alley + " ")) +
                         (string.IsNullOrEmpty(query.Road) ? "" :"ถนน " + (query.Road + " ")) +
                         (string.IsNullOrEmpty(query.SubDistrict) ? "" :"แขวง " + (_systemRepository.GetAll<SubDistrictEntity>().Where(x => x.SubDistrictCode.ToString().Equals(query.SubDistrict)).FirstOrDefault().SubDistrictNameTh + " ")) +
                         (string.IsNullOrEmpty(query.District) ? "" :"เขต " + (_systemRepository.GetAll<DistrictEntity>().Where(x => x.DistrictCode.ToString().Equals(query.District)).FirstOrDefault().DistrictNameTh + " ")) +
                         (string.IsNullOrEmpty(query.Province) ? "" :"จังหวัด " + (_systemRepository.GetAll<ProvinceEntity>().Where(x => x.ProvinceCode.ToString().Equals(query.Province)).FirstOrDefault().ProvinceNameTh + " ")) +
                         (string.IsNullOrEmpty(query.PostCode) ? "" :"รหัสไปรษณีย์ " + query.PostCode);

        var queryCustomer = quotation.Customer;
        var cusAddress = (string.IsNullOrEmpty(queryCustomer.Building) ? "" :" " + (queryCustomer.Building + " ")) +
                         (string.IsNullOrEmpty(queryCustomer.RoomNo) ? "" :" " + (queryCustomer.RoomNo + " ")) +
                         (string.IsNullOrEmpty(queryCustomer.Floor) ? "" :" " + (queryCustomer.Floor + " ")) +
                         (string.IsNullOrEmpty(queryCustomer.Village) ? "" :" " + (queryCustomer.Village + " ")) +
                         (string.IsNullOrEmpty(queryCustomer.No) ? "" :" " + (queryCustomer.No + " ")) +
                         (string.IsNullOrEmpty(queryCustomer.Moo) ? "" :" " + (queryCustomer.Moo + " ")) +
                         (string.IsNullOrEmpty(queryCustomer.Alley) ? "" :" " + (queryCustomer.Alley + " ")) +
                         (string.IsNullOrEmpty(queryCustomer.Road) ? "" :" " + (queryCustomer.Road + " ")) +
                         (string.IsNullOrEmpty(queryCustomer.SubDistrict) ? "" :queryCustomer.SubDistrict +
                         (string.IsNullOrEmpty(queryCustomer.District) ? "" :" " + queryCustomer.District +
                         (string.IsNullOrEmpty(queryCustomer.Province) ? "" :" " + "จังหวัด " + (_systemRepository.GetAll<ProvinceEntity>().Where(x => x.ProvinceCode.ToString().Equals(query.Province)).FirstOrDefault().ProvinceNameTh + " ")) +
                         (string.IsNullOrEmpty(queryCustomer.PostCode) ? "" :" " + queryCustomer.PostCode)));

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

        string HtmlContent =
            $"เร\u0e37\u0e48อง ขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49ใบเสนอราคา<br/>" +
            $"เร\u0e35ยน " +
            // $"{managerName}</br>" +
            $"<dd>เน\u0e37\u0e48องจากในขณะน\u0e35\u0e49เอกสารใบเสนอราคาเลขท\u0e35\u0e48: {quotation.QuotationNo ?? ""} ได\u0e49ถ\u0e39กจ\u0e31ดทำเสร\u0e47จเร\u0e35ยบร\u0e49อยแล\u0e49ว จ\u0e36งนำเสนอมาเพ\u0e37\u0e48อขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49รายละเอ\u0e35ยดท\u0e31\u0e49งหมดตามในเอกสารด\u0e31งกล\u0e48าวและจะได\u0e49" +
            $"ดำเน\u0e34นการเสนอราคาแก\u0e48ล\u0e39กค\u0e49าต\u0e48อไป\n</dd><br/><br/><br/>\n" +
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

    private async Task ManagerReplyApproveQuotation(QuotationEntity quotation, string saleName, string saleEmail)
    {
        var apiInstance = new TransactionalEmailsApi();
        string senderName = "PROM ERP";
        string senderEmail = "e-service@prom.co.th";

        SendSmtpEmailSender Email = new SendSmtpEmailSender(senderName, senderEmail);

        string toEmail = saleName;
        string toName = saleEmail;
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

        string HtmlContent = $"เร\u0e37\u0e48อง ขออน\u0e38ม\u0e31ต\u0e34เสนอ ราคาส\u0e38ทธ\u0e34/หน\u0e48วย ท\u0e35\u0e48ต\u0e48ำกว\u0e48าท\u0e35\u0e48ถ\u0e39กกำหนด<br/>" +
                             $"เร\u0e35ยน ผ\u0e39\u0e49ท\u0e35\u0e48เก\u0e35\u0e48ยวข\u0e49องท\u0e38กท\u0e48าน</br>" +
                             $"<dd>เน\u0e37\u0e48องจากในขณะน\u0e35\u0e49ม\u0e35ความจำเป\u0e47นบางประการท\u0e35\u0e48จะต\u0e49องเสนอ ราคาส\u0e38ทธ\u0e34/หน\u0e48วย ท\u0e35\u0e48ต\u0e48ำกว\u0e48า" +
                             $"ราคาต\u0e48ำส\u0e38ดซ\u0e36\u0e48งได\u0e49ถ\u0e39กกำหนดไว\u0e49ในระบบเพ\u0e37\u0e48อใช\u0e49เฉพาะก\u0e31บเอกสารใบเสนอราคาเลขท\u0e35\u0e48: " +
                             $"{entity.QuotationNo}" +
                             $"จ\u0e36งเร\u0e35ยนมาเพ\u0e37\u0e48อโปรดพ\u0e34จารณา<br/>\n" +
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
}