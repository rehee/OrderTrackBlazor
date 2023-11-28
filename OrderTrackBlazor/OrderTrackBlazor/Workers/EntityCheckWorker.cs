using Microsoft.EntityFrameworkCore;
using ReheeCmf.Commons;
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Workers
{
  public class EntityCheckWorker : BackgroundService
  {
    private readonly IServiceProvider sp;

    public EntityCheckWorker(IServiceProvider sp)
    {
      this.sp = sp;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      try
      {
        while (true)
        {

          await Task.Delay(1000 * 60);
          using var scope = sp.CreateScope();
          using var context = scope.ServiceProvider.GetService<IContext>();
          var current = DateTime.UtcNow;
          double deleteMins = -15;
          //double deleteMins = 0;
          var nextCurrent = DateTime.UtcNow.AddMinutes(deleteMins);

          var dispatchs = await context.Query<OrderTrackDispatchRecord>(false)
            .Where(b =>
              (b.SoftDeleteUntil != null && b.SoftDeleteUntil < current) ||
              (b.Status == EnumDispatchStatus.Error &&
                (
                  (b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
                )

            ).ToListAsync();
          foreach (var b in dispatchs)
          {

            context.Delete<OrderTrackDispatchRecord>(b);
          }
          await context.SaveChangesAsync(null);

          var emptyPurchase = context.Query<OrderTrackPurchaseRecord>(false).Where(b => b.Items.All(b => b.Quantity == 0) == true).ToList();
          foreach (var b in emptyPurchase)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
              if (b.Items?.Any() == true)
              {
                foreach (var b2 in b.Items)
                {
                  context.Delete<OrderTrackPurchaseItem>(b2);
                }
              }
              context.Delete<OrderTrackPurchaseRecord>(b);
            }
          }
          await context.SaveChangesAsync(null);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
