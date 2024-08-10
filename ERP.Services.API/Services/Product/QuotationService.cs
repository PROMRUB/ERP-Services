using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Quotation;
using ERP.Services.API.Utils;
using Microsoft.EntityFrameworkCore;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;

namespace ERP.Services.API.Services.Product;

public class QuotationService : IQuotationService
{
    private readonly IMapper _mapper;
    private readonly IQuotationRepository _quotationRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IPaymentAccountRepository _paymentAccountRepository;

    public QuotationService(IMapper mapper, IQuotationRepository quotationRepository,
        IProductRepository productRepository,
        IOrganizationRepository organizationRepository,
        IPaymentAccountRepository paymentAccountRepository)
    {
        try
        {
            _mapper = mapper;
            _quotationRepository = quotationRepository;
            _productRepository = productRepository;
            _organizationRepository = organizationRepository;
            _paymentAccountRepository = paymentAccountRepository;
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
            Discount = Convert.ToInt32(x.Discount)
        }).ToList();
    }

    public List<QuotationProjectResource> MapProjectEntityToResource(List<QuotationProjectEntity> entities)
    {
        return entities.Select(x => new QuotationProjectResource
        {
            ProjectId = x.ProjectId,
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
            Order = i,
        }).ToList();

        return products;
    }

    public List<QuotationProjectEntity> MutateResourceProject(List<QuotationProjectResource> resources)
    {
        var projects = resources.Select((x, i) => new QuotationProjectEntity
        {
            ProjectId = x.ProjectId,
            LeadTime = x.LeadTime,
            Warranty = x.Warranty,
            PaymentConditionId = x.ConditionId,
            Po = x.PurchaseOrder,
            Order = i
        }).ToList();

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
                PaymentId = resource.PaymentAccountId
            };

            quotation.SubmitStatus(resource.Status);
            quotation.Products = MutateResourceProduct(resource.Products);
            quotation.Projects = MutateResourceProject(resource.Projects);

            var result = await this.Calculate(resource.Products);

            quotation.Price = result.Price;
            quotation.Vat = result.Vat;
            quotation.Amount = result.Amount;

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
        quotation.Update();

        await _quotationRepository.Context()!.SaveChangesAsync();

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

        var products = MutateResourceProduct(resource);

        // foreach (var product in products)
        // {
        //     var selected = await _productRepository.GetProductListQueryable()
        //         .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
        //
        //     if (selected == null)
        //     {
        //         throw new KeyNotFoundException("product not exists");
        //     }
        // }

        price = (decimal)products.Sum(x => x.Amount * x.Quantity);

        amount = price * (decimal)1.07;
        vat = amount - price;


        var response = new QuotationResponse()
        {
            Amount = amount,
            Vat = vat,
            Price = price
        };

        return response;
    }

    public async Task<List<PaymentAccountResponse>> GetPaymentAccountListByBusiness(string orgId, Guid businessId)
    {
        _organizationRepository.SetCustomOrgId(orgId);
        var organization = await _organizationRepository.GetOrganization();
        var result = await _paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId)
            .Where(x => x.AccountStatus == RecordStatus.Active.ToString())
            .OrderBy(x => x.AccountBank).ToListAsync();
        return result.Select(x => new PaymentAccountResponse
        {
            PaymentAccountId = x.PaymentAccountId,
            OrgId = x.OrgId,
            PaymentAccountName = x.PaymentAccountName,
            AccountType = x.AccountType,
            AccountBank = x.AccountBank,
            // AccountBankName = x.AccountBank,
            AccountBrn = x.AccountBrn,
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

        var query = _quotationRepository.GetQuotationQuery()
                .Where(x => x.BusinessId == businessId)
                .Where(x => (string.IsNullOrWhiteSpace(keyword) || x.Customer.CusName.Contains(keyword))
                            && (string.IsNullOrWhiteSpace(keyword) || x.QuotationNo.Contains(keyword))
                            && (string.IsNullOrWhiteSpace(keyword) ||
                                x.Products.Any(p => p.Product.ProductName.Contains(keyword))) &&
                            (string.IsNullOrWhiteSpace(keyword) ||
                             x.QuotationNo.Contains(keyword)))
                .OrderBy(x => x.QuotationDateTime)
            ;


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
                Projects = null,
                Price = x.Price,
                Vat = x.Vat,
                Amount = x.Amount,
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

        var name = "อมรร\u0e31ตน\u0e4c เท\u0e35ยนบ\u0e38ญยาจารย\u0e4c";
        var email = "kkunayothin@gmail.com";

        await SendApproveSalePrice(quotation, name, email);


        return null;
    }

    public async Task<QuotationResource> ApproveQuotation(Guid id)
    {
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        var name = "อมรร\u0e31ตน\u0e4c เท\u0e35ยนบ\u0e38ญยาจารย\u0e4c";
        var email = "kkunayothin@gmail.com";
        try
        {
            await SendApproveQuotation(quotation, name, email
            );
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
        var quotation = await _quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        return new QuotationDocument(quotation);
    }

    private async Task SendApproveQuotation(QuotationEntity quotation, string managerName, string managerEmail)
    {
        Console.WriteLine("Environment.GetEnvironmentVariable(\"ERP_EMAIL\")");
        Console.WriteLine(Environment.GetEnvironmentVariable("ERP_EMAIL"));

        var apiInstance = new TransactionalEmailsApi();
        string SenderName = "PROM ERP";
        string SenderEmail = "e-service@prom.co.th";
        SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
        string ToEmail = managerEmail;
        string ToName = managerName;
        SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
        List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
        To.Add(smtpEmailTo);

        string HtmlContent =
            $"<h3 >ส\u0e48งเม\u0e37\u0e48อ: Sales ต\u0e49องการขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49ใบเสนอราคา</h3><br/>\n" +
            $"เร\u0e37\u0e48อง ขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49ใบเสนอราคา<br/>" +
            $"เร\u0e35ยน {managerName}</br>" +
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
        Console.WriteLine(Environment.GetEnvironmentVariable("ERP_EMAIL"));

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
        string senderName = "Admin";
        string senderEmail = "admin@prom.co.th";

        SendSmtpEmailSender Email = new SendSmtpEmailSender(senderName, senderEmail);

        string toEmail = quotation.CustomerContact!.Email!;
        string toName = quotation.CustomerContact!.DisplayName()!;
        SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(toEmail, toName);

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


    public static async Task SendEmail(QuotationEntity entity)
    {
        Configuration.Default.ApiKey.Add("api-key",
            Environment.GetEnvironmentVariable("ERP_EMAIL"));

        Console.WriteLine(Environment.GetEnvironmentVariable("ERP_EMAIL"));

        var apiInstance = new TransactionalEmailsApi();
        string SenderName = "PROM ERP";
        string SenderEmail = "e-service@prom.co.th";
        SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
        string ToEmail = "kkunayothin@gmail.com";
        string ToName = "korn";
        SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
        List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
        To.Add(smtpEmailTo);

        string HtmlContent = $"<h3 >ส\u0e48งเพ\u0e37\u0e48อขอ Approve</h3><br/>\n" +
                             $"<h3 >ส\u0e48งเม\u0e37\u0e48อ: Sales ต\u0e49องการขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49ใบเสนอราคา</h3><br/>\n" +
                             $"เร\u0e37\u0e48อง ขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49ใบเสนอราคา<br/>" +
                             $"เร\u0e35ยน XXX</br>" +
                             $"<dd>เน\u0e37\u0e48องจากในขณะน\u0e35\u0e49เอกสารใบเสนอราคาเลขท\u0e35\u0e48: QTYYYYMM-9999 ได\u0e49ถ\u0e39กจ\u0e31ดทำเสร\u0e47จเร\u0e35ยบร\u0e49อยแล\u0e49ว จ\u0e36งนำเสนอมาเพ\u0e37\u0e48อขออน\u0e38ม\u0e31ต\u0e34ใช\u0e49รายละเอ\u0e35ยดท\u0e31\u0e49งหมดตามในเอกสารด\u0e31งกล\u0e48าวและจะได\u0e49" +
                             $"ดำเน\u0e34นการเสนอราคาแก\u0e48ล\u0e39กค\u0e49าต\u0e48อไป\n</dd><br/><br/><br/>\n" +
                             $"จ\u0e36งเร\u0e35ยนมาเพ\u0e37\u0e48อโปรดพ\u0e34จารณา<br/>\n" +
                             $"{entity.IssuedByUser.DisplayNameTH()}<br/>";
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