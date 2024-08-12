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
            var formater = $"QT{DateTime.Now.ToString("yyyyMM")}-";

            var runner =
                dbContext.Quotation
                    .OrderBy(x => x.Running)
                    .LastOrDefault(
                    x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);


            var date = runner == null ? 1.ToString("D4") : (runner.Running + 1).ToString("D4");

            var running = runner == null ? 1 : runner.Running + 1;
            context.Entity.Running = running;
            context.Entity.QuotationNo = formater + date;
        }

        return Task.CompletedTask;
    }
}