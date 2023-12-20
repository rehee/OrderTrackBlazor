using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Data;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.EntityChangeHandlers;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackStockDispatch : BaseOrderTrackEntity
  {
    public DateTime? DispatchDate { get; set; }
    public EnumDispatchStatus? Status { get; set; }
    public decimal? Income { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? BriefNote { get; set; }
    public string? Note { get; set; }
    public virtual List<OrderTrackStockDispatchPackage>? Packages { get; set; }

  }

  [EntityChangeTracker<OrderTrackStockDispatch>]
  public class OrderTrackStockDispatchHandler : OrderTrackEntityHandler<OrderTrackStockDispatch>
  {
    public override async Task BeforeDeleteAsync(CancellationToken ct = default)
    {
      await base.BeforeDeleteAsync(ct);
      var packages = await context.Query<OrderTrackStockDispatchPackage>(false).Where(b => b.OrderTrackStockDispatchId == entity.Id).ToArrayAsync();
      foreach (var package in packages)
      {
        foreach (var item in await context.Query<OrderTrackStockDispatchPackageItem>(false).Where(b => b.PackageId == package.Id).ToArrayAsync())
        {
          context.Delete(item);
        }
        foreach (var item in await context.Query<OrderTrackDispatchItem>(false).Where(b => b.OrderTrackStockDispatchPackageId == package.Id).ToArrayAsync())
        {
          context.Delete(item);
        }
        context.Delete(package);
      }
    }
  }

}
