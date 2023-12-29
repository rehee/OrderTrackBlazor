using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Entities;
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
        using (var sc = sp.CreateScope())
        {
          using var context1 = sc.ServiceProvider.GetService<IContext>();
          var productions = await context1.Query<OrderTrackProduction>(false).ToArrayAsync();
          foreach (var production in productions)
          {
            production.NormalizationName = production.Name?.Trim().ToUpper() ?? "";
          }
          await context1.SaveChangesAsync(null);
        }
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
            .Where(b => b.Status == EnumDispatchStatus.Error)
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
            .Where(b => (b.Quantity + b.PackageQuantity) == 0).ToListAsync();

          foreach (var b in dispatchItemss)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
              context.Delete<OrderTrackDispatchItem>(b);
            }

          }
          await context.SaveChangesAsync(null);

          var emptyPurchaseItem = context.Query<OrderTrackPurchaseItem>(false)
            .Where(b =>
              (b.ProductionId == null || b.ProductionId <= 0) ||
              (b.PurchaseRecord != null && b.PurchaseRecord.OrderId != null && b.Quantity == 0)
            ).ToList();
          foreach (var b2 in emptyPurchaseItem)
          {
            if ((b2.UpdateDate != null && b2.UpdateDate < nextCurrent) ||
                  b2.CreateDate < nextCurrent)
            {
              context.Delete<OrderTrackPurchaseItem>(b2);
            }
          }
          await context.SaveChangesAsync(null);
          var emptyPurchase = context.Query<OrderTrackPurchaseRecord>(false).Where(b => b.Items.Any() != true || b.Items.All(b => b.Quantity == 0)).ToList();
          foreach (var b in emptyPurchase)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
              context.Delete<OrderTrackPurchaseRecord>(b);
            }
          }
          await context.SaveChangesAsync(null);

          var emptyOrderItem = await context.Query<OrderTrackOrderItem>(false)
            .Where(b => b.Quantity == 0).ToListAsync();

          foreach (var b in emptyOrderItem)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
              if (b.DispatchItems?.Any() == true)
              {
                foreach (var b2 in b.DispatchItems)
                {
                  b2.OrderProductionId = null;
                }
              }
              context.Delete<OrderTrackOrderItem>(b);
            }

          }
          await context.SaveChangesAsync(null);

          var emptyStockDispatchItem = await context.Query<OrderTrackStockDispatchPackageItem>(false)
            .Where(b => b.Number == 0).ToListAsync();
          foreach (var b in emptyStockDispatchItem)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
              foreach (var d in await context.Query<OrderTrackDispatchItem>(false)
                .Where(d => d.OrderTrackStockDispatchPackageId == b.PackageId).ToListAsync())
              {
                context.Delete(d);
              }
              context.Delete(b);
            }
          }
          await context.SaveChangesAsync(null);

          var emptyStockPackage = await context.Query<OrderTrackStockDispatchPackage>(false)
            .Where(b => b.Number == 0 && b.Items.Any() != true).ToListAsync();
          foreach (var b in emptyStockPackage)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
              context.Delete(b);
            }
          }
          await context.SaveChangesAsync(null);
          var errorDispatch = await context.Query<OrderTrackStockDispatch>(false).Where(b => b.Status == EnumDispatchStatus.Error).ToArrayAsync();
          foreach (var b in errorDispatch)
          {
            if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
                  b.CreateDate < nextCurrent)
            {
              context.Delete(b);
            }
          }
          await context.SaveChangesAsync(null);
          //var errorDispatchItem = await context.Query<OrderTrackDispatchItem>(false)
          //  .ToArrayAsync();
          //foreach (var b in errorDispatchItem)
          //{
          //  if ((b.UpdateDate != null && b.UpdateDate < nextCurrent) ||
          //        b.CreateDate < nextCurrent)
          //  {
          //    context.Delete(b);
          //  }
          //}
          //await context.SaveChangesAsync(null);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
