using ERP.Services.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.API.Interfaces;

public interface IQuotationRepository
{
    public IQueryable<QuotationEntity> GetQuotationQuery();
    public void Add(QuotationEntity entity);
    public void Delete(QuotationEntity entity);
    public void AddProduct(List<QuotationProductEntity> entity);
    public void AddProject(List<QuotationProjectEntity> entity);
    public void DeleteProduct(List<QuotationProductEntity> entity);
    public void DeleteProject(List<QuotationProjectEntity> entity);
    public DbContext? Context();
}