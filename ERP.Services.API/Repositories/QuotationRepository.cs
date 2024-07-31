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
            .Include(x => x.Projects)
            .Include(x => x.SalePerson)
            .Include(x => x.Customer)
            .Include(x => x.Customer)
            .Include(x => x.CustomerContact);
    }

    public void Add(QuotationEntity entity)
    {
        _context.Add(entity);
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
}