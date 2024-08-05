using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Quotation;
using ERP.Services.API.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;

namespace ERP.Services.API.Services.Product;

public class QuotationService
(IMapper mapper, IQuotationRepository quotationRepository, IProductRepository productRepository,
    IOrganizationRepository organizationRepository,
    IPaymentAccountRepository paymentAccountRepository) : IQuotationService
{
    public async Task<QuotationResource> GetQuotationById(string keyword)
    {
        var query = quotationRepository.GetQuotationQuery()
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

            quotationRepository.Add(quotation);

            try
            {
                await quotationRepository.Context().SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var entity = await quotationRepository.GetQuotationQuery()
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
        var quotation = await quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

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

        quotationRepository.DeleteProduct(quotation.Products);
        quotationRepository.DeleteProject(quotation.Projects);

        quotation.Products = MutateResourceProduct(resource.Products);
        quotation.Projects = MutateResourceProject(resource.Projects);
        quotation.Update();

        await quotationRepository.Context()!.SaveChangesAsync();

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
        var quotation = await quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        quotation.SubmitStatus(status.Status);

        await quotationRepository.Context()!.SaveChangesAsync();

        return MapEntityToResponse(quotation);
    }


    public async Task Process(Guid id)
    {
        var quotation = await quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        quotation.Process();

        await quotationRepository.Context()!.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var quotation = await quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        quotation.Process();

        await quotationRepository.Context()!.SaveChangesAsync();
    }

    public async Task<QuotationResponse> Calculate(List<QuotationProductResource> resource)
    {
        decimal amount;
        decimal vat;
        decimal price = 0;

        var products = MutateResourceProduct(resource);

        foreach (var product in products)
        {
            var selected = await productRepository.GetProductListQueryable()
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (selected == null)
            {
                throw new KeyNotFoundException("product not exists");
            }

            var discountPrice = (selected.MSRP * (product.Discount / 100) ?? 0);


            price += (discountPrice * product.Quantity);
        }

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
        organizationRepository.SetCustomOrgId(orgId);
        var organization = await organizationRepository.GetOrganization();
        var result = await paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId)
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
        organizationRepository.SetCustomOrgId(orgId);
        var organization = await organizationRepository.GetOrganization();
        var result = await paymentAccountRepository.GetPaymentAccountByBusiness((Guid)organization.OrgId, businessId)
            .Where(x => x.PaymentAccountId == paymentAccountId).FirstOrDefaultAsync();
        return mapper.Map<PaymentAccountEntity, PaymentAccountResponse>(result);
    }

    public async Task<PagedList<QuotationResponse>> GetByList(string keyword, Guid businessId, int page, int pageSize)
    {
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.ToLower();
        }

        var query = quotationRepository.GetQuotationQuery()
                .Where(x => x.BusinessId == businessId)
                .Where(x => (string.IsNullOrWhiteSpace(keyword) || x.Customer.CusName.Contains(keyword))
                            && (string.IsNullOrWhiteSpace(keyword) || x.QuotationNo.Contains(keyword))
                            && (string.IsNullOrWhiteSpace(keyword) ||
                                x.Products.Any(p => p.Product.ProductName.Contains(keyword)))
                )
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
        var quotation = await quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        return MapEntityToResponse(quotation);
    }

    public async Task<QuotationResource> ApproveSalePrice(Guid id)
    {
        await SendEmail();

        return null;
    }

    public Task<QuotationResource> ApproveQuotation(Guid id)
    {
        throw new NotImplementedException();
    }

    public static async Task SendEmail()
    {
        
    }
}