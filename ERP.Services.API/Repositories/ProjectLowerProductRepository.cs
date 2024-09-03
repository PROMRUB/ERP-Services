// using ERP.Services.API.Entities;
// using ERP.Services.API.Interfaces;
// using ERP.Services.API.PromServiceDbContext;
// using Microsoft.EntityFrameworkCore;
//
// namespace ERP.Services.API.Repositories;
//
// public class ProjectLowerProductRepository : BaseRepository, IProjectLowerProductRepository
// {
//     private readonly PromDbContext _context;
//
//     public ProjectLowerProductRepository(PromDbContext context)
//     {
//         _context = context;
//     }
//
//     public IQueryable<ProjectLowerPriceProductEntity> GetProjectLowerProductQuery()
//     {
//         return _context.ProjectLowerPriceProduct
//             .Include(x => x.Project)
//             .Include(x => x.Product);
//     }
//
//     public void Add(ProjectLowerPriceProductEntity entity)
//     {
//         _context.Add(entity);
//     }
//
//     public DbContext Context()
//     {
//         return _context;
//     }
// }