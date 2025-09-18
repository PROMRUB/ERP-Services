using ERP.Services.API.Entities;
using ERP.Services.API.Interfaces;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Repositories;

public class QuotationRepository : BaseRepository, IQuotationRepository
{
    private readonly PromDbContext _context;

    public QuotationRepository(PromDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<QuotationEntity> GetQuotationQuery()
    {
        return _context.Quotation
            .Include(x => x.Products)
            .ThenInclude(x => x.Product)
            .Include(x => x.Projects)
            .ThenInclude(x => x.Project)
            .Include(x => x.Projects)
            .ThenInclude(x => x.PaymentCondition)
            .Include(x => x.SalePerson)
            .Include(x => x.IssuedByUser)
            .Include(x => x.Customer)
            .Include(x => x.CustomerContact);
    }

    public void Add(QuotationEntity entity)
    {
        _context.Add(entity);
    }

    public IQueryable<QuotationProductEntity> GetQuotationProduct(Guid quotationId, Guid productId)
    {
        return _context.QuotationProduct.Where(x => x.ProductId == productId && x.QuotationId == quotationId);
    }

    public IQueryable<QuotationProductEntity> GetQuotationProducts(Guid productId)
    {
        return _context.QuotationProduct.Where(x => x.ProductId == productId);
    }
    
    public void UpdateProduct(QuotationProductEntity entity)
    {
        _context.QuotationProduct.Update(entity);
    }

    public void Delete(QuotationEntity entity)
    {
        _context.Remove(entity);
    }

    public void AddProduct(List<QuotationProductEntity> entity)
    {
        _context.QuotationProduct.AddRange(entity);
    }

    public void AddProject(List<QuotationProjectEntity> entity)
    {
        _context.QuotationProject.AddRange(entity);
    }

    public void DeleteProduct(List<QuotationProductEntity> entity)
    {
        _context.QuotationProduct.RemoveRange(entity);
    }

    public void DeleteProject(List<QuotationProjectEntity> entity)
    {
        _context.QuotationProject.RemoveRange(entity);
    }

    public DbContext Context()
    {
        return _context;
    }

    public void DeleteAll(List<QuotationEntity> query)
    {
        _context.RemoveRange(query);
    }
}