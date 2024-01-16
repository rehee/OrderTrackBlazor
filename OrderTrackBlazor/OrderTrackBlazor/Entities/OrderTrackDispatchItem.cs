using ReheeCmf.Components.ChangeComponents;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackDispatchItem : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackDispatchRecord))]
    public long? DispatchRecordId { get; set; }
    public virtual OrderTrackDispatchRecord? DispatchRecord { get; set; }


    [ForeignKey(nameof(OrderTrackProduction))]
    public long? ProductionId { get; set; }
    public virtual OrderTrackProduction? Production { get; set; }

    [ForeignKey(nameof(OrderTrackOrderItem))]
    public long? OrderProductionId { get; set; }
    public virtual OrderTrackOrderItem? OrderProduction { get; set; }

    public decimal? DispatchPrice { get; set; }
    public int Quantity { get; set; }
    public int PackageQuantity { get; set; }

    [ForeignKey(nameof(OrderTrackStockDispatchPackage))]
    public long? OrderTrackStockDispatchPackageId { get; set; }
    public virtual OrderTrackStockDispatchPackage? OrderTrackStockDispatchPackage { get; set; }
  }
  [EntityChangeTracker<OrderTrackDispatchItem>]
  public class OrderTrackDispatchItemItemHandler : OrderTrackEntityHandler<OrderTrackDispatchItem>
  {
    public override async Task AfterCreateAsync(CancellationToken ct = default)
    {
      await base.AfterCreateAsync(ct);
      if (entity.OrderProduction != null)
      {
        await entity.OrderProduction.OverDeliveredCheck(context);
      }
    }
    public override async Task AfterUpdateAsync(CancellationToken ct = default)
    {
      await base.AfterUpdateAsync(ct);
      if (entity.OrderProduction != null)
      {
        await entity.OrderProduction.OverDeliveredCheck(context);
      }
    }
    public override async Task AfterDeleteAsync(CancellationToken ct = default)
    {
      await base.AfterDeleteAsync(ct);
      if (entity.OrderProduction != null)
      {
        await entity.OrderProduction.OverDeliveredCheck(context);
      }
    }
  }
}
