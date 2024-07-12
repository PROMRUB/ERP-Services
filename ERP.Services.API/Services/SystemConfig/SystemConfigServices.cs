using AutoMapper;
using ERP.Services.API.Entities;
using ERP.Services.API.Enum;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.ResponseModels.SystemConfig;
using ERP.Services.API.Repositories;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace ERP.Services.API.Services.SystemConfig
{
    public class SystemConfigServices : ISystemConfigServices
    {
        private readonly IMapper mapper;
        private readonly ISystemConfigRepository repository;

        public SystemConfigServices(IMapper mapper,
            ISystemConfigRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<List<ProvinceResponse>> GetProvincesAsync()
        {
            var query = await repository.GetAll<ProvinceEntity>().ToListAsync();
            return mapper.Map<List<ProvinceEntity>, List<ProvinceResponse>>(query);
        }

        public async Task<List<DistrictResponse>> GetDistrictsAsync()
        {
            var query = await repository.GetAll<DistrictEntity>().ToListAsync();
            return mapper.Map<List<DistrictEntity>, List<DistrictResponse>>(query);
        }

        public async Task<List<SubDIstrictResponse>> GetSubDistrictsAsync()
        {
            var query = await repository.GetAll<SubDistrictEntity>().ToListAsync();
            return mapper.Map<List<SubDistrictEntity>, List<SubDIstrictResponse>>(query);
        }

        public async Task<List<BankResponse>> GetBanksAsync()
        {
            var query = await repository.GetAll<BankEntity>().ToListAsync();
            return mapper.Map<List<BankEntity>, List<BankResponse>>(query);
        }

        public async Task<List<BankBranchResponse>> GetBankBranchsAsync()
        {
            var query = await repository.GetAll<BankBranchEntity>().ToListAsync();
            return mapper.Map<List<BankBranchEntity>, List<BankBranchResponse>>(query);
        }

        public async Task ImportBank(IFormFile request)
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var items = new List<BankEntity>();

                using (var stream = new MemoryStream())
                {
                    request.CopyTo(stream);
                    stream.Position = 0;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            items.Add(new BankEntity
                            {
                                BankId = Guid.NewGuid(),
                                BankCode = worksheet.Cells[row, 1].Text,
                                BankAbbr = worksheet.Cells[row, 2].Text,
                                BankTHName = worksheet.Cells[row, 3].Text,
                                BankENName = worksheet.Cells[row, 4].Text
                            });
                        }
                        stream.Dispose();
                    }
                }

                foreach (var item in items)
                {
                    repository.AddAsync(item);
                }
                repository.Commit();
            }
            catch
            {
                throw;
            }
        }

        public async Task ImportBankBranch(IFormFile request)
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var items = new List<BankBranchEntity>();

                using (var stream = new MemoryStream())
                {
                    request.CopyTo(stream);
                    stream.Position = 0;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            items.Add(new BankBranchEntity
                            {
                                 BankBranchId = Guid.NewGuid(),
                                BankCode = worksheet.Cells[row, 1].Text,
                                BankBranchCode = worksheet.Cells[row, 2].Text,
                                BankBranchTHName = worksheet.Cells[row, 3].Text,
                                BankBranchENName = worksheet.Cells[row, 4].Text
                            });
                        }
                        stream.Dispose();
                    }
                }

                foreach (var item in items)
                {
                    repository.AddAsync(item);
                }
                repository.Commit();
            }
            catch
            {
                throw;
            }
        }
    }
}
