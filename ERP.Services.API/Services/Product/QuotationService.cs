using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Quotation;
using ERP.Services.API.Models.ResponseModels.Quotation;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Services.Product;

public class QuotationService
    (IQuotationRepository quotationRepository, IProductRepository productRepository) : IQuotationService
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
        var response = new QuotationResponse
        {
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
            Price = quotation.Price.ToString(),
            Vat = quotation.Vat.ToString(),
            Amount = quotation.Amount.ToString(),
            AccountNo = quotation.AccountNo
        };

        return response;
    }

    public List<QuotationProductEntity> MutateResourceProduct(List<QuotationProductResource> resources)
    {
        var products = resources.Select((x, i) => new QuotationProductEntity
        {
            ProductId = x.ProductId,
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
        var quotation = new QuotationEntity
        {
            EditTime = 0,
            CustomerId = resource.CustomerId.Value,
            CustomerContactId = resource.ContactPersonId.Value,
            QuotationDateTime = DateTime.Now,
            SalePersonId = resource.SalePersonId.Value,
            IssuedById = resource.IssuedById.Value,
            IsApproved = false,
        };

        quotation.Products = MutateResourceProduct(resource.Products);
        quotation.Projects = MutateResourceProject(resource.Projects);

        quotationRepository.Add(quotation);

        await quotationRepository.Context()!.SaveChangesAsync();

        //todo : amount

        var entity = await quotationRepository.GetQuotationQuery()
            .FirstOrDefaultAsync(x => x.QuotationId == quotation.QuotationId);

        var response = new QuotationResponse();

        return response;
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
            new() { Status = "เสนอราคา" },
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

            decimal calPrice = product.Price > (selected.LwPrice ?? (decimal)0)
                ? (decimal)selected.LwPrice!
                : (decimal)product.Price;

            price += (calPrice * product.Quantity);
        }

        amount = price * (decimal)1.07;
        vat = amount - price;


        var response = new QuotationResponse()
        {
            Products = MapProductEntityToResource(products),
            Amount = amount.ToString(),
            Vat = vat.ToString(),
            Price = price.ToString()
        };

        return response;
    }
}