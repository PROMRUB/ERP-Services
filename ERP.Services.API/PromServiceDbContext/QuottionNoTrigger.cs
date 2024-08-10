using EntityFrameworkCore.Triggered;
using ERP.Services.API.Entities;

namespace ERP.Services.API.PromServiceDbContext;

public class QuotationNoTrigger(PromDbContext dbContext) : IBeforeSaveTrigger<QuotationEntity>
{
    private readonly PromDbContext promContext = dbContext;

    public Task BeforeSave(ITriggerContext<QuotationEntity> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Added)
        {
            var formater = $"QT-{DateTime.Now.ToString("yyyyMM")}";

            var runner =
                dbContext.Quotation.FirstOrDefault(
                    x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);


            var date = runner == null ? 1.ToString("D4") : runner.Running.ToString("D4");

            context.Entity.Running += 1;
            context.Entity.QuotationNo = formater + date;
        }

        return Task.CompletedTask;
    }
}