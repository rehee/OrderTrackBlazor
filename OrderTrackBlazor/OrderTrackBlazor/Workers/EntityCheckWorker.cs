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

          await Task.Delay(1000
          //);
          * 60);
          using var scope = sp.CreateScope();
          using var context = scope.ServiceProvider.GetService<IContext>();
          var current = DateTime.UtcNow;
          double deleteMins = -15;
          //double deleteMins = 0;
          var nextCurrent = DateTime.UtcNow.AddMinutes(deleteMins);

          var dispatchs = await context.Query<OrderTrackDispatchRecord>(false)
            .Where(b => b.Status== EnumDispatchStatus.Error)
             .ToListAsync();
          foreach (var b in dispatchs)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
              context.Delete<OrderTrackDispatchRecord>(b);
            }
          }
          await context.SaveChangesAsync(null);

          var dispatchItemss = await context.Query<OrderTrackDispatchItem>(false)
            .Where(b => b.Quantity == 0).ToListAsync();

          foreach (var b in dispatchItemss)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
              context.Delete<OrderTrackDispatchItem>(b);
            }
            
          }
          await context.SaveChangesAsync(null);

          var emptyPurchaseItem = context.Query<OrderTrackPurchaseItem>(false).Where(b => b.Quantity == 0).ToList();
          foreach (var b2 in emptyPurchaseItem)
          {
            if ((b2.UpdateDate != null && b2.UpdateDate < nextCurrent) ||
                  b2.CreateDate < nextCurrent)
            {
              context.Delete<OrderTrackPurchaseItem>(b2);
            }
          }
          await context.SaveChangesAsync(null);
          var emptyPurchase = context.Query<OrderTrackPurchaseRecord>(false).Where(b => b.Items.Any() != true).ToList();
          foreach (var b in emptyPurchase)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
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
