using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.PaymentAccount;
using ERP.Services.API.Models.ResponseModels.Quotation;
using ERP.Services.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Services.Product;

public class QuotationService
(IMapper mapper, IQuotationRepository quotationRepository, IProductRepository productRepository,
    IOrganizationRepository organizationRepository,
    IPaymentAccountRepository paymentAccountRepository) : IQuotationService
{
    public async Task<QuotationResponse> GetQuotationById(string keyword)
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

    public List<QuotationProductResponse> MapProductEntityToResource(List<QuotationProductEntity> entities)
    {
        return entities.Select(x => new QuotationProductResponse
        {
            ProductId = x.ProductId,
            ProductName = x.Product.ProductName,
            Quantity = x.Quantity,
            // Unit = x.Product.Unit,
            Amount = x.Amount,
            BasePrice = x.Product.LwPrice.Value,
            Order = x.Order
        }).ToList();
    }

    public List<QuotationProjectResponse> MapProjectEntityToResource(List<QuotationProjectEntity> entities)
    {
        return entities.Select(x => new QuotationProjectResponse
        {
            ProjectId = x.ProjectId,
            ProjectName = x.Project.ProjectName,
            LeadTime = x.LeadTime,
            Warranty = x.Warranty,
            PurchaseOrder = x.Po,
            Order = x.Order
        }).ToList();
    }

    public QuotationResponse MapEntityToResponse(QuotationEntity quotation)
    {
        try
        {
            var response = new QuotationResponse
            {
                QuotationId = quotation.QuotationId.Value,
                CustomerId = quotation.CustomerId,
                CustomerNo = quotation.Customer.CusCustomId,
                CustomerName = quotation.Customer.DisplayName,
                Address = quotation.Customer.Address(),
                ContactPerson = quotation.CustomerContact.DisplayName(),
                ContactPersonId = quotation.CustomerContactId,
                QuotationNo = quotation.QuotationNo,
                QuotationDateTime = quotation.QuotationDateTime.ToString("dd/MM/yyyyy"),
                EditTime = quotation.EditTime,
                IssuedByUser = quotation.IssuedById,
                IssuedByUserName = quotation.IssuedByUser.DisplayName(),
                SalePersonId = quotation.SalePersonId,
                SalePersonIName = quotation.SalePerson.DisplayName(),
                Status = quotation.Status,
                Products = MapProductEntityToResource(quotation.Products),
                Projects = MapProjectEntityToResource(quotation.Projects),
                Price = quotation.Price,
                Vat = quotation.Vat,
                Amount = quotation.Amount,
                AccountNo = quotation.PaymentId ?? Guid.NewGuid(),
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
            PaymentCondition = x.PaymentCondition,
            Po = x.PurchaseOrder,
            Order = i
        }).ToList();

        return projects;
    }

    public async Task<QuotationResponse> Create(QuotationResource resource)
    {
        try
        {
            var quotation = new QuotationEntity
            {
                EditTime = 0,
                CustomerId = resource.CustomerId,
                CustomerContactId = resource.ContactPersonId,
                QuotationDateTime = DateTime.UtcNow,
                SalePersonId = null,
                IssuedById = null,
                IsApproved = false,
                Remark = resource.Remark,
                BusinessId = resource.BusinessId,
                Status = resource.Status
            };

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

    public async Task<QuotationResponse> Update(Guid id, QuotationResource resource)
    {
        var quotation = await quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }


        quotationRepository.DeleteProduct(quotation.Products);
        quotationRepository.DeleteProject(quotation.Projects);

        quotation.Products = MutateResourceProduct(resource.Products);
        quotation.Projects = MutateResourceProject(resource.Projects);


        await quotationRepository.Context()!.SaveChangesAsync();

        return MapEntityToResponse(quotation);
    }

    public Task<List<QuotationStatus>> QuotationStatus()
    {
        return Task.FromResult(new List<QuotationStatus>()
        {
            new() { Status = "รอการอนุมติ" },
            new() { Status = "ยกเลิก" },
            new() { Status = "ปิดการขาย" },
        });
    }

    public async Task<QuotationResponse> UpdateStatus(Guid id, string status)
    {
        var quotation = await quotationRepository.GetQuotationQuery().FirstOrDefaultAsync(x => x.QuotationId == id);

        if (quotation == null)
        {
            throw new KeyNotFoundException("id not exists");
        }

        quotation.SubmitStatus(status);

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
    }

    public async Task<QuotationResponse> Calculate(List<QuotationProductResource> resource)
    {
        decimal amount = 0;
        decimal vat = 0;
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
            // Products = MapProductEntityToResource(products),
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
        keyword = keyword.ToLower();
        var query = quotationRepository.GetQuotationQuery()
            .Where(x => x.Customer.CusName.Contains(keyword)
                        || x.QuotationNo.Contains(keyword) ||
                        x.Products.Any(p => p.Product.ProductName.Contains(keyword)));


        var pagedList = await PagedList<Entities.QuotationEntity>.Create(query, page, pageSize);


        throw new Exception();
    }

    public Task<QuotationResponse> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}